
vec4 getPosition() {
vec4 pos = position;
#if defined(TARGET_VULKAN_ENVIRONMENT)
pos.y = -pos.y;
#endif
return pos;
}
