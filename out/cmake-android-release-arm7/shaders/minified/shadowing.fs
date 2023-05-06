
#define SHADOW_SAMPLING_RUNTIME_PCF     0u
#define SHADOW_SAMPLING_RUNTIME_EVSM    1u
#define SHADOW_SAMPLING_RUNTIME_DPCF    2u
#define SHADOW_SAMPLING_RUNTIME_PCSS    3u
#define SHADOW_SAMPLING_PCF_HARD        0
#define SHADOW_SAMPLING_PCF_LOW         1
#define SHADOW_SAMPLING_METHOD          SHADOW_SAMPLING_PCF_LOW
float sampleDepth(const mediump sampler2DArrayShadow map,
const highp vec4 scissorNormalized,
const uint layer,  highp vec2 uv, float depth) {
uv = clamp(uv, scissorNormalized.xy, scissorNormalized.zw);
return texture(map, vec4(uv, layer, saturate(depth)));
}
#if SHADOW_SAMPLING_METHOD == SHADOW_SAMPLING_PCF_HARD
float ShadowSample_PCF_Hard(const mediump sampler2DArrayShadow map,
const highp vec4 scissorNormalized,
const uint layer, const highp vec4 shadowPosition) {
highp vec3 position = shadowPosition.xyz * (1.0 / shadowPosition.w);
return sampleDepth(map, scissorNormalized, layer, position.xy, position.z);
}
#endif
#if SHADOW_SAMPLING_METHOD == SHADOW_SAMPLING_PCF_LOW
float ShadowSample_PCF_Low(const mediump sampler2DArrayShadow map,
const highp vec4 scissorNormalized,
const uint layer, const highp vec4 shadowPosition) {
highp vec3 position = shadowPosition.xyz * (1.0 / shadowPosition.w);
highp vec2 size = vec2(textureSize(map, 0));
highp vec2 texelSize = vec2(1.0) / size;
float depth = position.z;
position.xy = clamp(position.xy, vec2(-1.0), vec2(2.0));
vec2 offset = vec2(0.5);
highp vec2 uv = (position.xy * size) + offset;
highp vec2 base = (floor(uv) - offset) * texelSize;
highp vec2 st = fract(uv);
vec2 uw = vec2(3.0 - 2.0 * st.x, 1.0 + 2.0 * st.x);
vec2 vw = vec2(3.0 - 2.0 * st.y, 1.0 + 2.0 * st.y);
highp vec2 u = vec2((2.0 - st.x) / uw.x - 1.0, st.x / uw.y + 1.0);
highp vec2 v = vec2((2.0 - st.y) / vw.x - 1.0, st.y / vw.y + 1.0);
u *= texelSize.x;
v *= texelSize.y;
float sum = 0.0;
sum += uw.x * vw.x * sampleDepth(map, scissorNormalized, layer, base + vec2(u.x, v.x), depth);
sum += uw.y * vw.x * sampleDepth(map, scissorNormalized, layer, base + vec2(u.y, v.x), depth);
sum += uw.x * vw.y * sampleDepth(map, scissorNormalized, layer, base + vec2(u.x, v.y), depth);
sum += uw.y * vw.y * sampleDepth(map, scissorNormalized, layer, base + vec2(u.y, v.y), depth);
return sum * (1.0 / 16.0);
}
#endif
float ShadowSample_PCF(const mediump sampler2DArray map,
const highp vec4 scissorNormalized,
const uint layer, const highp vec4 shadowPosition) {
highp vec3 position = shadowPosition.xyz * (1.0 / shadowPosition.w);
highp vec2 size = vec2(textureSize(map, 0));
highp vec2 tc = clamp(position.xy, scissorNormalized.xy, scissorNormalized.zw);
highp vec2 st = tc.xy * size - 0.5;
vec4 d;
#if defined(FILAMENT_HAS_FEATURE_TEXTURE_GATHER)
d = textureGather(map, vec3(tc, layer), 0);
#else
d[0] = texelFetchOffset(map, ivec3(st, layer), 0, ivec2(0, 1)).r;
d[1] = texelFetchOffset(map, ivec3(st, layer), 0, ivec2(1, 1)).r;
d[2] = texelFetchOffset(map, ivec3(st, layer), 0, ivec2(1, 0)).r;
d[3] = texelFetchOffset(map, ivec3(st, layer), 0, ivec2(0, 0)).r;
#endif
vec4 pcf = step(0.0, position.zzzz - d);
highp vec2 grad = fract(st);
return mix(mix(pcf.w, pcf.z, grad.x), mix(pcf.x, pcf.y, grad.x), grad.y);
}
mediump vec2 poissonDisk[64] = vec2[](
vec2(0.511749, 0.547686), vec2(0.58929, 0.257224), vec2(0.165018, 0.57663), vec2(0.407692, 0.742285),
vec2(0.707012, 0.646523), vec2(0.31463, 0.466825), vec2(0.801257, 0.485186), vec2(0.418136, 0.146517),
vec2(0.579889, 0.0368284), vec2(0.79801, 0.140114), vec2(-0.0413185, 0.371455), vec2(-0.0529108, 0.627352),
vec2(0.0821375, 0.882071), vec2(0.17308, 0.301207), vec2(-0.120452, 0.867216), vec2(0.371096, 0.916454),
vec2(-0.178381, 0.146101), vec2(-0.276489, 0.550525), vec2(0.12542, 0.126643), vec2(-0.296654, 0.286879),
vec2(0.261744, -0.00604975), vec2(-0.213417, 0.715776), vec2(0.425684, -0.153211), vec2(-0.480054, 0.321357),
vec2(-0.0717878, -0.0250567), vec2(-0.328775, -0.169666), vec2(-0.394923, 0.130802), vec2(-0.553681, -0.176777),
vec2(-0.722615, 0.120616), vec2(-0.693065, 0.309017), vec2(0.603193, 0.791471), vec2(-0.0754941, -0.297988),
vec2(0.109303, -0.156472), vec2(0.260605, -0.280111), vec2(0.129731, -0.487954), vec2(-0.537315, 0.520494),
vec2(-0.42758, 0.800607), vec2(0.77309, -0.0728102), vec2(0.908777, 0.328356), vec2(0.985341, 0.0759158),
vec2(0.947536, -0.11837), vec2(-0.103315, -0.610747), vec2(0.337171, -0.584), vec2(0.210919, -0.720055),
vec2(0.41894, -0.36769), vec2(-0.254228, -0.49368), vec2(-0.428562, -0.404037), vec2(-0.831732, -0.189615),
vec2(-0.922642, 0.0888026), vec2(-0.865914, 0.427795), vec2(0.706117, -0.311662), vec2(0.545465, -0.520942),
vec2(-0.695738, 0.664492), vec2(0.389421, -0.899007), vec2(0.48842, -0.708054), vec2(0.760298, -0.62735),
vec2(-0.390788, -0.707388), vec2(-0.591046, -0.686721), vec2(-0.769903, -0.413775), vec2(-0.604457, -0.502571),
vec2(-0.557234, 0.00451362), vec2(0.147572, -0.924353), vec2(-0.0662488, -0.892081), vec2(0.863832, -0.407206)
);
const uint DPCF_SHADOW_TAP_COUNT                = 12u;
const uint PCSS_SHADOW_BLOCKER_SEARCH_TAP_COUNT = 16u;
const uint PCSS_SHADOW_FILTER_TAP_COUNT         = 16u;
float hardenedKernel(float x) {
x = 2.0 * x - 1.0;
float s = sign(x);
x = 1.0 - s * x;
x = x * x * x;
x = s - x * s;
return 0.5 * x + 0.5;
}
highp vec2 computeReceiverPlaneDepthBias(const highp vec3 position) {
highp vec3 duvz_dx = dFdx(position);
highp vec3 duvz_dy = dFdy(position);
highp vec2 dz_duv = inverse(transpose(mat2(duvz_dx.xy, duvz_dy.xy))) * vec2(duvz_dx.z, duvz_dy.z);
return dz_duv;
}
mat2 getRandomRotationMatrix(highp vec2 fragCoord) {
fragCoord += vec2(frameUniforms.temporalNoise);
float randomAngle = interleavedGradientNoise(fragCoord) * (2.0 * PI);
vec2 randomBase = vec2(cos(randomAngle), sin(randomAngle));
mat2 R = mat2(randomBase.x, randomBase.y, -randomBase.y, randomBase.x);
return R;
}
float getPenumbraLs(const bool DIRECTIONAL, const uint index, const highp float zLight) {
float penumbra;
if (DIRECTIONAL) {
penumbra = shadowUniforms.shadows[index].bulbRadiusLs;
} else {
penumbra = shadowUniforms.shadows[index].bulbRadiusLs / zLight;
}
return penumbra;
}
float getPenumbraRatio(const bool DIRECTIONAL, const uint index,
float z_receiver, float z_blocker) {
float penumbraRatio;
if (DIRECTIONAL) {
penumbraRatio = (z_blocker - z_receiver) / (1.0 - z_blocker);
} else {
float nearOverFarMinusNear = shadowUniforms.shadows[index].nearOverFarMinusNear;
penumbraRatio = (nearOverFarMinusNear + z_blocker) / (nearOverFarMinusNear + z_receiver) - 1.0;
}
return penumbraRatio * frameUniforms.shadowPenumbraRatioScale;
}
void blockerSearchAndFilter(out float occludedCount, out float z_occSum,
const mediump sampler2DArray map, const highp vec4 scissorNormalized, const highp vec2 uv,
const float z_rec, const uint layer,
const highp vec2 filterRadii, const mat2 R, const highp vec2 dz_duv,
const uint tapCount) {
occludedCount = 0.0;
z_occSum = 0.0;
for (uint i = 0u; i < tapCount; i++) {
highp vec2 duv = R * (poissonDisk[i] * filterRadii);
highp vec2 tc = clamp(uv + duv, scissorNormalized.xy, scissorNormalized.zw);
float z_occ = textureLod(map, vec3(tc, layer), 0.0).r;
float z_bias = dot(dz_duv, duv);
float dz = z_occ - z_rec;
float occluded = step(z_bias, dz);
occludedCount += occluded;
z_occSum += z_occ * occluded;
}
}
float filterPCSS(const mediump sampler2DArray map,
const highp vec4 scissorNormalized,
const highp vec2 size,
const highp vec2 uv, const float z_rec, const uint layer,
const highp vec2 filterRadii, const mat2 R, const highp vec2 dz_duv,
const uint tapCount) {
float occludedCount = 0.0;
for (uint i = 0u; i < tapCount; i++) {
highp vec2 duv = R * (poissonDisk[i] * filterRadii);
vec4 d;
highp vec2 tc = clamp(uv + duv, scissorNormalized.xy, scissorNormalized.zw);
highp vec2 st = tc.xy * size - 0.5;
highp vec2 grad = fract(st);
#if defined(FILAMENT_HAS_FEATURE_TEXTURE_GATHER)
d = textureGather(map, vec3(tc, layer), 0);
#else
d[0] = texelFetchOffset(map, ivec3(st, layer), 0, ivec2(0, 1)).r;
d[1] = texelFetchOffset(map, ivec3(st, layer), 0, ivec2(1, 1)).r;
d[2] = texelFetchOffset(map, ivec3(st, layer), 0, ivec2(1, 0)).r;
d[3] = texelFetchOffset(map, ivec3(st, layer), 0, ivec2(0, 0)).r;
#endif
float z_bias = dot(dz_duv, duv);
vec4 dz = d - vec4(z_rec);
vec4 pcf = step(z_bias, dz);
occludedCount += mix(mix(pcf.w, pcf.z, grad.x), mix(pcf.x, pcf.y, grad.x), grad.y);
}
return occludedCount * (1.0 / float(tapCount));
}
float ShadowSample_DPCF(const bool DIRECTIONAL,
const mediump sampler2DArray map,
const highp vec4 scissorNormalized,
const uint layer, const uint index,
const highp vec4 shadowPosition, const highp float zLight) {
highp vec3 position = shadowPosition.xyz * (1.0 / shadowPosition.w);
highp vec2 texelSize = vec2(1.0) / vec2(textureSize(map, 0));
highp vec2 dz_duv = computeReceiverPlaneDepthBias(position);
float penumbra = getPenumbraLs(DIRECTIONAL, index, zLight);
mat2 R = getRandomRotationMatrix(gl_FragCoord.xy);
float occludedCount = 0.0;
float z_occSum = 0.0;
blockerSearchAndFilter(occludedCount, z_occSum,
map, scissorNormalized, position.xy, position.z, layer, texelSize * penumbra, R, dz_duv,
DPCF_SHADOW_TAP_COUNT);
if (z_occSum == 0.0) {
return 1.0;
}
float penumbraRatio = getPenumbraRatio(DIRECTIONAL, index, position.z, z_occSum / occludedCount);
penumbraRatio = saturate(penumbraRatio);
float percentageOccluded = occludedCount * (1.0 / float(DPCF_SHADOW_TAP_COUNT));
percentageOccluded = mix(hardenedKernel(percentageOccluded), percentageOccluded, penumbraRatio);
return 1.0 - percentageOccluded;
}
float ShadowSample_PCSS(const bool DIRECTIONAL,
const mediump sampler2DArray map,
const highp vec4 scissorNormalized,
const uint layer, const uint index,
const highp vec4 shadowPosition, const highp float zLight) {
highp vec2 size = vec2(textureSize(map, 0));
highp vec2 texelSize = vec2(1.0) / size;
highp vec3 position = shadowPosition.xyz * (1.0 / shadowPosition.w);
highp vec2 dz_duv = computeReceiverPlaneDepthBias(position);
float penumbra = getPenumbraLs(DIRECTIONAL, index, zLight);
mat2 R = getRandomRotationMatrix(gl_FragCoord.xy);
float occludedCount = 0.0;
float z_occSum = 0.0;
blockerSearchAndFilter(occludedCount, z_occSum,
map, scissorNormalized, position.xy, position.z, layer, texelSize * penumbra, R, dz_duv,
PCSS_SHADOW_BLOCKER_SEARCH_TAP_COUNT);
if (z_occSum == 0.0) {
return 1.0;
}
float penumbraRatio = getPenumbraRatio(DIRECTIONAL, index, position.z, z_occSum / occludedCount);
float percentageOccluded = filterPCSS(map, scissorNormalized, size,
position.xy, position.z, layer,
texelSize * (penumbra * penumbraRatio),
R, dz_duv, PCSS_SHADOW_FILTER_TAP_COUNT);
return 1.0 - percentageOccluded;
}
struct ScreenSpaceRay {
highp vec3 ssRayStart;
highp vec3 ssRayEnd;
highp vec3 ssViewRayEnd;
highp vec3 uvRayStart;
highp vec3 uvRay;
};
void initScreenSpaceRay(out ScreenSpaceRay ray, highp vec3 wsRayStart, vec3 wsRayDirection, float wsRayLength) {
highp mat4 worldToClip = getClipFromWorldMatrix();
highp mat4 viewToClip = getClipFromViewMatrix();
highp vec3 wsRayEnd = wsRayStart + wsRayDirection * wsRayLength;
highp vec4 csRayStart = worldToClip * vec4(wsRayStart, 1.0);
highp vec4 csRayEnd = worldToClip * vec4(wsRayEnd, 1.0);
highp vec4 csViewRayEnd = csRayStart + viewToClip * vec4(0.0, 0.0, wsRayLength, 0.0);
ray.ssRayStart = csRayStart.xyz * (1.0 / csRayStart.w);
ray.ssRayEnd = csRayEnd.xyz * (1.0 / csRayEnd.w);
ray.ssViewRayEnd = csViewRayEnd.xyz * (1.0 / csViewRayEnd.w);
highp vec3 uvRayEnd = vec3(ray.ssRayEnd.xy * 0.5 + 0.5, ray.ssRayEnd.z);
ray.uvRayStart = vec3(ray.ssRayStart.xy * 0.5 + 0.5, ray.ssRayStart.z);
ray.uvRay = uvRayEnd - ray.uvRayStart;
}
float screenSpaceContactShadow(vec3 lightDirection) {
float occlusion = 0.0;
uint kStepCount = (frameUniforms.directionalShadows >> 8u) & 0xFFu;
float kDistanceMax = frameUniforms.ssContactShadowDistance;
ScreenSpaceRay rayData;
initScreenSpaceRay(rayData, shading_position, lightDirection, kDistanceMax);
highp float dt = 1.0 / float(kStepCount);
highp float tolerance = abs(rayData.ssViewRayEnd.z - rayData.ssRayStart.z) * dt;
float dither = interleavedGradientNoise(gl_FragCoord.xy) - 0.5;
highp float t = dt * dither + dt;
highp vec3 ray;
for (uint i = 0u ; i < kStepCount ; i++, t += dt) {
ray = rayData.uvRayStart + rayData.uvRay * t;
highp float z = textureLod(light_structure, uvToRenderTargetUV(ray.xy), 0.0).r;
highp float dz = z - ray.z;
if (abs(tolerance - dz) < tolerance) {
occlusion = 1.0;
break;
}
}
vec2 fade = max(12.0 * abs(ray.xy - 0.5) - 5.0, 0.0);
occlusion *= saturate(1.0 - dot(fade, fade));
return occlusion;
}
float linstep(const float min, const float max, const float v) {
return clamp((v - min) / (max - min), 0.0, 1.0);
}
float reduceLightBleed(const float pMax, const float amount) {
return linstep(amount, 1.0, pMax);
}
float chebyshevUpperBound(const highp vec2 moments, const highp float mean,
const highp float minVariance, const float lightBleedReduction) {
highp float variance = moments.y - (moments.x * moments.x);
variance = max(variance, minVariance);
highp float d = mean - moments.x;
float pMax = variance / (variance + d * d);
pMax = reduceLightBleed(pMax, lightBleedReduction);
return mean <= moments.x ? 1.0 : pMax;
}
float evaluateShadowVSM(const highp vec2 moments, const highp float depth) {
highp float depthScale = frameUniforms.vsmDepthScale * depth;
highp float minVariance = depthScale * depthScale;
return chebyshevUpperBound(moments, depth, minVariance, frameUniforms.vsmLightBleedReduction);
}
float ShadowSample_VSM(const bool ELVSM, const mediump sampler2DArray shadowMap,
const highp vec4 scissorNormalized,
const uint layer, const highp vec4 shadowPosition) {
highp vec3 position = vec3(shadowPosition.xy * (1.0 / shadowPosition.w), shadowPosition.z);
highp vec4 moments = texture(shadowMap, vec3(position.xy, layer));
highp float depth = position.z;
depth = depth * 2.0 - 1.0;
depth = frameUniforms.vsmExponent * depth;
depth = exp(depth);
float p = evaluateShadowVSM(moments.xy, depth);
if (ELVSM) {
p = min(p, evaluateShadowVSM(moments.zw, -1.0 / depth));
}
return p;
}
#if defined(VARIANT_HAS_DIRECTIONAL_LIGHTING)
highp vec4 getShadowPosition(const uint cascade) {
return getCascadeLightSpacePosition(cascade);
}
#endif
#if defined(VARIANT_HAS_DYNAMIC_LIGHTING)
highp vec4 getShadowPosition(const uint index,  const highp vec3 dir, const highp float zLight) {
return getSpotLightSpacePosition(index, dir, zLight);
}
#endif
uint getPointLightFace(const highp vec3 r) {
highp vec4 tc;
highp float rx = abs(r.x);
highp float ry = abs(r.y);
highp float rz = abs(r.z);
highp float d = max(rx, max(ry, rz));
if (d == rx) {
return (r.x >= 0.0 ? 0u : 1u);
} else if (d == ry) {
return (r.y >= 0.0 ? 2u : 3u);
} else {
return (r.z >= 0.0 ? 4u : 5u);
}
}
float shadow(const bool DIRECTIONAL,
const mediump sampler2DArrayShadow shadowMap,
const uint index, highp vec4 shadowPosition, highp float zLight) {
highp vec4 scissorNormalized = shadowUniforms.shadows[index].scissorNormalized;
uint layer = shadowUniforms.shadows[index].layer;
#if SHADOW_SAMPLING_METHOD == SHADOW_SAMPLING_PCF_HARD
return ShadowSample_PCF_Hard(shadowMap, scissorNormalized, layer, shadowPosition);
#elif SHADOW_SAMPLING_METHOD == SHADOW_SAMPLING_PCF_LOW
return ShadowSample_PCF_Low(shadowMap, scissorNormalized, layer, shadowPosition);
#endif
}
float shadow(const bool DIRECTIONAL,
const mediump sampler2DArray shadowMap,
const uint index, highp vec4 shadowPosition, highp float zLight) {
highp vec4 scissorNormalized = shadowUniforms.shadows[index].scissorNormalized;
uint layer = shadowUniforms.shadows[index].layer;
if (frameUniforms.shadowSamplingType == SHADOW_SAMPLING_RUNTIME_EVSM) {
bool elvsm = shadowUniforms.shadows[index].elvsm;
return ShadowSample_VSM(elvsm, shadowMap, scissorNormalized, layer,
shadowPosition);
}
if (frameUniforms.shadowSamplingType == SHADOW_SAMPLING_RUNTIME_DPCF) {
return ShadowSample_DPCF(DIRECTIONAL, shadowMap, scissorNormalized, layer, index,
shadowPosition, zLight);
}
if (frameUniforms.shadowSamplingType == SHADOW_SAMPLING_RUNTIME_PCSS) {
return ShadowSample_PCSS(DIRECTIONAL, shadowMap, scissorNormalized, layer, index,
shadowPosition, zLight);
}
if (frameUniforms.shadowSamplingType == SHADOW_SAMPLING_RUNTIME_PCF) {
return ShadowSample_PCF(shadowMap, scissorNormalized,
layer, shadowPosition);
}
return 0.0;
}
