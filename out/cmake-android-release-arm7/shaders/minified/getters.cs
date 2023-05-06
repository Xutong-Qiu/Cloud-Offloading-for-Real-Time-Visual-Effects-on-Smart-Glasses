
uvec3 getWorkGroupID() {
return gl_WorkGroupID;
}
uvec3 getWorkGroupCount() {
return gl_NumWorkGroups;
}
uvec3 getLocalInvocationID() {
return gl_LocalInvocationID;
}
uint getLocalInvocationIndex() {
return gl_LocalInvocationIndex;
}
uvec3 getGlobalInvocationID() {
return gl_GlobalInvocationID;
}
