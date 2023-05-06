
PerRenderableData object_uniforms;
void initObjectUniforms(out PerRenderableData p) {
#if defined(MATERIAL_HAS_INSTANCES)
p = objectUniforms.data[0];
#else
p.worldFromModelMatrix = objectUniforms.data[instance_index].worldFromModelMatrix;
p.worldFromModelNormalMatrix = objectUniforms.data[instance_index].worldFromModelNormalMatrix;
p.morphTargetCount = objectUniforms.data[instance_index].morphTargetCount;
p.flagsChannels = objectUniforms.data[instance_index].flagsChannels;
p.objectId = objectUniforms.data[instance_index].objectId;
p.userData = objectUniforms.data[instance_index].userData;
#endif
}
#if defined(MATERIAL_HAS_INSTANCES)
int getInstanceIndex() {
return instance_index;
}
#endif
