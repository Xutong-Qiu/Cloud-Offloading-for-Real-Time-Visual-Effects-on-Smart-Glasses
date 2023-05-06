
#define FROXEL_BUFFER_WIDTH_SHIFT   6u
#define FROXEL_BUFFER_WIDTH         (1u << FROXEL_BUFFER_WIDTH_SHIFT)
#define FROXEL_BUFFER_WIDTH_MASK    (FROXEL_BUFFER_WIDTH - 1u)
#define LIGHT_TYPE_POINT            0u
#define LIGHT_TYPE_SPOT             1u
struct FroxelParams {
uint recordOffset;
uint count;
};
uvec3 getFroxelCoords(const highp vec3 fragCoords) {
uvec3 froxelCoord;
froxelCoord.xy = uvec2(fragCoords.xy * frameUniforms.froxelCountXY);
highp float viewSpaceNormalizedZ = frameUniforms.zParams.x * fragCoords.z + frameUniforms.zParams.y;
float zSliceCount = frameUniforms.zParams.w;
float sliceZWithoutOffset = log2(viewSpaceNormalizedZ) * frameUniforms.zParams.z;
froxelCoord.z = uint(clamp(sliceZWithoutOffset + zSliceCount, 0.0, zSliceCount - 1.0));
return froxelCoord;
}
uint getFroxelIndex(const highp vec3 fragCoords) {
uvec3 froxelCoord = getFroxelCoords(fragCoords);
return froxelCoord.x * frameUniforms.fParams.x +
froxelCoord.y * frameUniforms.fParams.y +
froxelCoord.z * frameUniforms.fParams.z;
}
ivec2 getFroxelTexCoord(uint froxelIndex) {
return ivec2(froxelIndex & FROXEL_BUFFER_WIDTH_MASK, froxelIndex >> FROXEL_BUFFER_WIDTH_SHIFT);
}
FroxelParams getFroxelParams(uint froxelIndex) {
ivec2 texCoord = getFroxelTexCoord(froxelIndex);
uvec2 entry = texelFetch(light_froxels, texCoord, 0).rg;
FroxelParams froxel;
froxel.recordOffset = entry.r;
froxel.count = entry.g & 0xFFu;
return froxel;
}
uint getLightIndex(const uint index) {
uint v = index >> 4u;
uint c = (index >> 2u) & 0x3u;
uint s = (index & 0x3u) * 8u;
highp uvec4 d = froxelRecordUniforms.records[v];
return (d[c] >> s) & 0xFFu;
}
float getSquareFalloffAttenuation(float distanceSquare, float falloff) {
float factor = distanceSquare * falloff;
float smoothFactor = saturate(1.0 - factor * factor);
return smoothFactor * smoothFactor;
}
float getDistanceAttenuation(const highp vec3 posToLight, float falloff) {
float distanceSquare = dot(posToLight, posToLight);
float attenuation = getSquareFalloffAttenuation(distanceSquare, falloff);
highp vec3 v = getWorldPosition() - getWorldCameraPosition();
float d = dot(v, v);
attenuation *= saturate(frameUniforms.lightFarAttenuationParams.x - d * frameUniforms.lightFarAttenuationParams.y);
return attenuation / max(distanceSquare, 1e-4);
}
float getAngleAttenuation(const highp vec3 lightDir, const highp vec3 l, const highp vec2 scaleOffset) {
float cd = dot(lightDir, l);
float attenuation = saturate(cd * scaleOffset.x + scaleOffset.y);
return attenuation * attenuation;
}
Light getLight(const uint lightIndex) {
highp mat4 data = lightsUniforms.lights[lightIndex];
highp vec4 positionFalloff = data[0];
highp vec3 direction = data[1].xyz;
vec4 colorIES = vec4(
unpackHalf2x16(floatBitsToUint(data[2][0])),
unpackHalf2x16(floatBitsToUint(data[2][1]))
);
highp vec2 scaleOffset = data[2].zw;
highp float intensity = data[3][1];
highp uint typeShadow = floatBitsToUint(data[3][2]);
highp uint channels = floatBitsToUint(data[3][3]);
highp vec3 worldPosition = getWorldPosition();
highp vec3 posToLight = positionFalloff.xyz - worldPosition;
Light light;
light.colorIntensity.rgb = colorIES.rgb;
light.colorIntensity.w = computePreExposedIntensity(intensity, frameUniforms.exposure);
light.l = normalize(posToLight);
light.attenuation = getDistanceAttenuation(posToLight, positionFalloff.w);
light.direction = direction;
light.NoL = saturate(dot(shading_normal, light.l));
light.worldPosition = positionFalloff.xyz;
light.channels = channels;
light.contactShadows = bool(typeShadow & 0x10u);
#if defined(VARIANT_HAS_DYNAMIC_LIGHTING)
light.type = (typeShadow & 0x1u);
#if defined(VARIANT_HAS_SHADOWING)
light.shadowIndex = (typeShadow >>  8u) & 0xFFu;
light.castsShadows   = bool(channels & 0x10000u);
if (light.type == LIGHT_TYPE_SPOT) {
light.zLight = dot(shadowUniforms.shadows[light.shadowIndex].lightFromWorldZ, vec4(worldPosition, 1.0));
}
#endif
if (light.type == LIGHT_TYPE_SPOT) {
light.attenuation *= getAngleAttenuation(-direction, light.l, scaleOffset);
}
#endif
return light;
}
void evaluatePunctualLights(const MaterialInputs material,
const PixelParams pixel, inout vec3 color) {
FroxelParams froxel = getFroxelParams(getFroxelIndex(getNormalizedPhysicalViewportCoord()));
uint index = froxel.recordOffset;
uint end = index + froxel.count;
uint channels = object_uniforms.flagsChannels & 0xFFu;
for ( ; index < end; index++) {
uint lightIndex = getLightIndex(index);
Light light = getLight(lightIndex);
if ((light.channels & channels) == 0u) {
continue;
}
#if defined(MATERIAL_CAN_SKIP_LIGHTING)
if (light.NoL <= 0.0 || light.attenuation <= 0.0) {
continue;
}
#endif
float visibility = 1.0;
#if defined(VARIANT_HAS_SHADOWING)
if (light.NoL > 0.0) {
if (light.castsShadows) {
uint shadowIndex = light.shadowIndex;
if (light.type == LIGHT_TYPE_POINT) {
highp vec3 r = getWorldPosition() - light.worldPosition;
uint face = getPointLightFace(r);
shadowIndex += face;
light.zLight = dot(shadowUniforms.shadows[shadowIndex].lightFromWorldZ,
vec4(getWorldPosition(), 1.0));
}
highp vec4 shadowPosition = getShadowPosition(shadowIndex, light.direction, light.zLight);
visibility = shadow(false, light_shadowMap, shadowIndex,
shadowPosition, light.zLight);
}
if (light.contactShadows && visibility > 0.0) {
if ((object_uniforms.flagsChannels & FILAMENT_OBJECT_CONTACT_SHADOWS_BIT) != 0u) {
visibility *= 1.0 - screenSpaceContactShadow(light.l);
}
}
#if defined(MATERIAL_CAN_SKIP_LIGHTING)
if (visibility <= 0.0) {
continue;
}
#endif
}
#endif
#if defined(MATERIAL_HAS_CUSTOM_SURFACE_SHADING)
color.rgb += customSurfaceShading(material, pixel, light, visibility);
#else
color.rgb += surfaceShading(pixel, light, visibility);
#endif
}
}
