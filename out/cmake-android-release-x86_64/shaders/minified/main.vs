
void main() {
#if defined(TARGET_METAL_ENVIRONMENT) || defined(TARGET_VULKAN_ENVIRONMENT)
instance_index = gl_InstanceIndex;
#else
instance_index = gl_InstanceID;
#endif
initObjectUniforms(object_uniforms);
#if defined(USE_OPTIMIZED_DEPTH_VERTEX_SHADER)
#if !defined(VERTEX_DOMAIN_DEVICE) || defined(VARIANT_HAS_VSM)
MaterialVertexInputs material;
initMaterialVertex(material);
materialVertex(material);
#endif
#else
MaterialVertexInputs material;
initMaterialVertex(material);
#if defined(HAS_ATTRIBUTE_TANGENTS)
#if defined(MATERIAL_NEEDS_TBN)
toTangentFrame(mesh_tangents, material.worldNormal, vertex_worldTangent.xyz);
#if defined(VARIANT_HAS_SKINNING_OR_MORPHING)
if ((object_uniforms.flagsChannels & FILAMENT_OBJECT_MORPHING_ENABLED_BIT) != 0u) {
#if defined(LEGACY_MORPHING)
vec3 normal0, normal1, normal2, normal3;
toTangentFrame(mesh_custom4, normal0);
toTangentFrame(mesh_custom5, normal1);
toTangentFrame(mesh_custom6, normal2);
toTangentFrame(mesh_custom7, normal3);
vec3 baseNormal = material.worldNormal;
material.worldNormal += morphingUniforms.weights[0].xyz * (normal0 - baseNormal);
material.worldNormal += morphingUniforms.weights[1].xyz * (normal1 - baseNormal);
material.worldNormal += morphingUniforms.weights[2].xyz * (normal2 - baseNormal);
material.worldNormal += morphingUniforms.weights[3].xyz * (normal3 - baseNormal);
#else
morphNormal(material.worldNormal);
material.worldNormal = normalize(material.worldNormal);
#endif
}
if ((object_uniforms.flagsChannels & FILAMENT_OBJECT_SKINNING_ENABLED_BIT) != 0u) {
skinNormal(material.worldNormal, mesh_bone_indices, mesh_bone_weights);
skinNormal(vertex_worldTangent.xyz, mesh_bone_indices, mesh_bone_weights);
}
#endif
vertex_worldTangent.xyz = getWorldFromModelNormalMatrix() * vertex_worldTangent.xyz;
vertex_worldTangent.w = mesh_tangents.w;
material.worldNormal = getWorldFromModelNormalMatrix() * material.worldNormal;
#else
toTangentFrame(mesh_tangents, material.worldNormal);
#if defined(VARIANT_HAS_SKINNING_OR_MORPHING)
if ((object_uniforms.flagsChannels & FILAMENT_OBJECT_MORPHING_ENABLED_BIT) != 0u) {
#if defined(LEGACY_MORPHING)
vec3 normal0, normal1, normal2, normal3;
toTangentFrame(mesh_custom4, normal0);
toTangentFrame(mesh_custom5, normal1);
toTangentFrame(mesh_custom6, normal2);
toTangentFrame(mesh_custom7, normal3);
vec3 baseNormal = material.worldNormal;
material.worldNormal += morphingUniforms.weights[0].xyz * (normal0 - baseNormal);
material.worldNormal += morphingUniforms.weights[1].xyz * (normal1 - baseNormal);
material.worldNormal += morphingUniforms.weights[2].xyz * (normal2 - baseNormal);
material.worldNormal += morphingUniforms.weights[3].xyz * (normal3 - baseNormal);
#else
morphNormal(material.worldNormal);
material.worldNormal = normalize(material.worldNormal);
#endif
}
if ((object_uniforms.flagsChannels & FILAMENT_OBJECT_SKINNING_ENABLED_BIT) != 0u) {
skinNormal(material.worldNormal, mesh_bone_indices, mesh_bone_weights);
}
#endif
material.worldNormal = getWorldFromModelNormalMatrix() * material.worldNormal;
#endif
#endif
materialVertex(material);
#if defined(HAS_ATTRIBUTE_COLOR)
vertex_color = material.color;
#endif
#if defined(HAS_ATTRIBUTE_UV0)
vertex_uv01.xy = material.uv0;
#endif
#if defined(HAS_ATTRIBUTE_UV1)
vertex_uv01.zw = material.uv1;
#endif
#if defined(VARIABLE_CUSTOM0)
VARIABLE_CUSTOM_AT0 = material.VARIABLE_CUSTOM0;
#endif
#if defined(VARIABLE_CUSTOM1)
VARIABLE_CUSTOM_AT1 = material.VARIABLE_CUSTOM1;
#endif
#if defined(VARIABLE_CUSTOM2)
VARIABLE_CUSTOM_AT2 = material.VARIABLE_CUSTOM2;
#endif
#if defined(VARIABLE_CUSTOM3)
VARIABLE_CUSTOM_AT3 = material.VARIABLE_CUSTOM3;
#endif
vertex_worldPosition.xyz = material.worldPosition.xyz;
#ifdef HAS_ATTRIBUTE_TANGENTS
vertex_worldNormal = material.worldNormal;
#endif
#if defined(VARIANT_HAS_SHADOWING) && defined(VARIANT_HAS_DIRECTIONAL_LIGHTING)
vertex_lightSpacePosition = computeLightSpacePosition(
vertex_worldPosition.xyz, vertex_worldNormal,
frameUniforms.lightDirection,
shadowUniforms.shadows[0].normalBias,
shadowUniforms.shadows[0].lightFromWorldMatrix);
#endif
#endif
#if defined(VERTEX_DOMAIN_DEVICE)
gl_Position = getPosition();
#if !defined(USE_OPTIMIZED_DEPTH_VERTEX_SHADER)
#if defined(MATERIAL_HAS_CLIP_SPACE_TRANSFORM)
gl_Position = getMaterialClipSpaceTransform(material) * gl_Position;
#endif
#endif
#if defined(MATERIAL_HAS_VERTEX_DOMAIN_DEVICE_JITTERED)
gl_Position.xy = gl_Position.xy * frameUniforms.clipTransform.xy + (gl_Position.w * frameUniforms.clipTransform.zw);
#endif
#else
gl_Position = getClipFromWorldMatrix() * getWorldPosition(material);
#endif
#if defined(VERTEX_DOMAIN_DEVICE)
gl_Position.z = gl_Position.z * -0.5 + 0.5;
#endif
#if defined(VARIANT_HAS_VSM)
highp float z = (getViewFromWorldMatrix() * getWorldPosition(material)).z;
highp float depth = -z * frameUniforms.oneOverFarMinusNear - frameUniforms.nearOverFarMinusNear;
depth = depth * 2.0 - 1.0;
vertex_worldPosition.w = depth;
#endif
vertex_position = gl_Position;
#if defined(TARGET_VULKAN_ENVIRONMENT)
gl_Position.y = -gl_Position.y;
#endif
#if !defined(TARGET_VULKAN_ENVIRONMENT) && !defined(TARGET_METAL_ENVIRONMENT)
gl_Position.z = dot(gl_Position.zw, frameUniforms.clipControl);
#endif
}
