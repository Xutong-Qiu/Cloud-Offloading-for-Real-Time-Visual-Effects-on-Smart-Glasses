void main() {
PostProcessVertexInputs inputs;
initPostProcessMaterialVertex(inputs);
inputs.normalizedUV = position.xy * 0.5 + 0.5;
gl_Position = getPosition();
gl_Position.z = gl_Position.z * -0.5 + 0.5;
#if !defined(TARGET_VULKAN_ENVIRONMENT) && !defined(TARGET_METAL_ENVIRONMENT)
gl_Position.z = dot(gl_Position.zw, frameUniforms.clipControl);
#endif
postProcessVertex(inputs);
#if defined(VARIABLE_CUSTOM0)
VARIABLE_CUSTOM_AT0 = inputs.VARIABLE_CUSTOM0;
#endif
#if defined(VARIABLE_CUSTOM1)
VARIABLE_CUSTOM_AT1 = inputs.VARIABLE_CUSTOM1;
#endif
#if defined(VARIABLE_CUSTOM2)
VARIABLE_CUSTOM_AT2 = inputs.VARIABLE_CUSTOM2;
#endif
#if defined(VARIABLE_CUSTOM3)
VARIABLE_CUSTOM_AT3 = inputs.VARIABLE_CUSTOM3;
#endif
}
