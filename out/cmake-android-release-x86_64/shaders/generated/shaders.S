    .global SHADERS_AMBIENT_OCCLUSION_FS_OFFSET;
    .global SHADERS_AMBIENT_OCCLUSION_FS_SIZE;
    .global SHADERS_ATTRIBUTES_VS_OFFSET;
    .global SHADERS_ATTRIBUTES_VS_SIZE;
    .global SHADERS_BRDF_FS_OFFSET;
    .global SHADERS_BRDF_FS_SIZE;
    .global SHADERS_COMMON_DEFINES_GLSL_OFFSET;
    .global SHADERS_COMMON_DEFINES_GLSL_SIZE;
    .global SHADERS_COMMON_GETTERS_GLSL_OFFSET;
    .global SHADERS_COMMON_GETTERS_GLSL_SIZE;
    .global SHADERS_COMMON_INSTANCING_GLSL_OFFSET;
    .global SHADERS_COMMON_INSTANCING_GLSL_SIZE;
    .global SHADERS_COMMON_GRAPHICS_FS_OFFSET;
    .global SHADERS_COMMON_GRAPHICS_FS_SIZE;
    .global SHADERS_COMMON_LIGHTING_FS_OFFSET;
    .global SHADERS_COMMON_LIGHTING_FS_SIZE;
    .global SHADERS_COMMON_MATERIAL_FS_OFFSET;
    .global SHADERS_COMMON_MATERIAL_FS_SIZE;
    .global SHADERS_COMMON_MATH_GLSL_OFFSET;
    .global SHADERS_COMMON_MATH_GLSL_SIZE;
    .global SHADERS_COMMON_SHADING_FS_OFFSET;
    .global SHADERS_COMMON_SHADING_FS_SIZE;
    .global SHADERS_COMMON_SHADOWING_GLSL_OFFSET;
    .global SHADERS_COMMON_SHADOWING_GLSL_SIZE;
    .global SHADERS_COMMON_TYPES_GLSL_OFFSET;
    .global SHADERS_COMMON_TYPES_GLSL_SIZE;
    .global SHADERS_DEPTH_MAIN_FS_OFFSET;
    .global SHADERS_DEPTH_MAIN_FS_SIZE;
    .global SHADERS_FOG_FS_OFFSET;
    .global SHADERS_FOG_FS_SIZE;
    .global SHADERS_GETTERS_CS_OFFSET;
    .global SHADERS_GETTERS_CS_SIZE;
    .global SHADERS_GETTERS_FS_OFFSET;
    .global SHADERS_GETTERS_FS_SIZE;
    .global SHADERS_GETTERS_VS_OFFSET;
    .global SHADERS_GETTERS_VS_SIZE;
    .global SHADERS_LIGHT_DIRECTIONAL_FS_OFFSET;
    .global SHADERS_LIGHT_DIRECTIONAL_FS_SIZE;
    .global SHADERS_LIGHT_INDIRECT_FS_OFFSET;
    .global SHADERS_LIGHT_INDIRECT_FS_SIZE;
    .global SHADERS_LIGHT_REFLECTIONS_FS_OFFSET;
    .global SHADERS_LIGHT_REFLECTIONS_FS_SIZE;
    .global SHADERS_LIGHT_PUNCTUAL_FS_OFFSET;
    .global SHADERS_LIGHT_PUNCTUAL_FS_SIZE;
    .global SHADERS_MAIN_CS_OFFSET;
    .global SHADERS_MAIN_CS_SIZE;
    .global SHADERS_MAIN_FS_OFFSET;
    .global SHADERS_MAIN_FS_SIZE;
    .global SHADERS_MAIN_VS_OFFSET;
    .global SHADERS_MAIN_VS_SIZE;
    .global SHADERS_MATERIAL_INPUTS_FS_OFFSET;
    .global SHADERS_MATERIAL_INPUTS_FS_SIZE;
    .global SHADERS_MATERIAL_INPUTS_VS_OFFSET;
    .global SHADERS_MATERIAL_INPUTS_VS_SIZE;
    .global SHADERS_POST_PROCESS_FS_OFFSET;
    .global SHADERS_POST_PROCESS_FS_SIZE;
    .global SHADERS_POST_PROCESS_VS_OFFSET;
    .global SHADERS_POST_PROCESS_VS_SIZE;
    .global SHADERS_POST_PROCESS_GETTERS_VS_OFFSET;
    .global SHADERS_POST_PROCESS_GETTERS_VS_SIZE;
    .global SHADERS_POST_PROCESS_INPUTS_FS_OFFSET;
    .global SHADERS_POST_PROCESS_INPUTS_FS_SIZE;
    .global SHADERS_POST_PROCESS_INPUTS_VS_OFFSET;
    .global SHADERS_POST_PROCESS_INPUTS_VS_SIZE;
    .global SHADERS_SHADING_LIT_FS_OFFSET;
    .global SHADERS_SHADING_LIT_FS_SIZE;
    .global SHADERS_SHADING_LIT_CUSTOM_FS_OFFSET;
    .global SHADERS_SHADING_LIT_CUSTOM_FS_SIZE;
    .global SHADERS_SHADING_MODEL_CLOTH_FS_OFFSET;
    .global SHADERS_SHADING_MODEL_CLOTH_FS_SIZE;
    .global SHADERS_SHADING_MODEL_STANDARD_FS_OFFSET;
    .global SHADERS_SHADING_MODEL_STANDARD_FS_SIZE;
    .global SHADERS_SHADING_MODEL_SUBSURFACE_FS_OFFSET;
    .global SHADERS_SHADING_MODEL_SUBSURFACE_FS_SIZE;
    .global SHADERS_SHADING_PARAMETERS_FS_OFFSET;
    .global SHADERS_SHADING_PARAMETERS_FS_SIZE;
    .global SHADERS_SHADING_REFLECTIONS_FS_OFFSET;
    .global SHADERS_SHADING_REFLECTIONS_FS_SIZE;
    .global SHADERS_SHADING_UNLIT_FS_OFFSET;
    .global SHADERS_SHADING_UNLIT_FS_SIZE;
    .global SHADERS_SHADOWING_FS_OFFSET;
    .global SHADERS_SHADOWING_FS_SIZE;
    .global SHADERS_VARYINGS_GLSL_OFFSET;
    .global SHADERS_VARYINGS_GLSL_SIZE;
    .global SHADERS_VIGNETTE_FS_OFFSET;
    .global SHADERS_VIGNETTE_FS_SIZE;

    .global SHADERS_PACKAGE
    .section .rodata
SHADERS_PACKAGE:
    .incbin "shaders.bin"
SHADERS_AMBIENT_OCCLUSION_FS_OFFSET:
    .int 0
SHADERS_AMBIENT_OCCLUSION_FS_SIZE:
    .int 5809
SHADERS_ATTRIBUTES_VS_OFFSET:
    .int 5809
SHADERS_ATTRIBUTES_VS_SIZE:
    .int 1466
SHADERS_BRDF_FS_OFFSET:
    .int 7275
SHADERS_BRDF_FS_SIZE:
    .int 5603
SHADERS_COMMON_DEFINES_GLSL_OFFSET:
    .int 12878
SHADERS_COMMON_DEFINES_GLSL_SIZE:
    .int 445
SHADERS_COMMON_GETTERS_GLSL_OFFSET:
    .int 13323
SHADERS_COMMON_GETTERS_GLSL_SIZE:
    .int 1495
SHADERS_COMMON_INSTANCING_GLSL_OFFSET:
    .int 14818
SHADERS_COMMON_INSTANCING_GLSL_SIZE:
    .int 700
SHADERS_COMMON_GRAPHICS_FS_OFFSET:
    .int 15518
SHADERS_COMMON_GRAPHICS_FS_SIZE:
    .int 1331
SHADERS_COMMON_LIGHTING_FS_OFFSET:
    .int 16849
SHADERS_COMMON_LIGHTING_FS_SIZE:
    .int 2420
SHADERS_COMMON_MATERIAL_FS_OFFSET:
    .int 19269
SHADERS_COMMON_MATERIAL_FS_SIZE:
    .int 1469
SHADERS_COMMON_MATH_GLSL_OFFSET:
    .int 20738
SHADERS_COMMON_MATH_GLSL_SIZE:
    .int 2574
SHADERS_COMMON_SHADING_FS_OFFSET:
    .int 23312
SHADERS_COMMON_SHADING_FS_SIZE:
    .int 375
SHADERS_COMMON_SHADOWING_GLSL_OFFSET:
    .int 23687
SHADERS_COMMON_SHADOWING_GLSL_SIZE:
    .int 408
SHADERS_COMMON_TYPES_GLSL_OFFSET:
    .int 24095
SHADERS_COMMON_TYPES_GLSL_SIZE:
    .int 607
SHADERS_DEPTH_MAIN_FS_OFFSET:
    .int 24702
SHADERS_DEPTH_MAIN_FS_SIZE:
    .int 1319
SHADERS_FOG_FS_OFFSET:
    .int 26021
SHADERS_FOG_FS_SIZE:
    .int 1725
SHADERS_GETTERS_CS_OFFSET:
    .int 27746
SHADERS_GETTERS_CS_SIZE:
    .int 300
SHADERS_GETTERS_FS_OFFSET:
    .int 28046
SHADERS_GETTERS_FS_SIZE:
    .int 2700
SHADERS_GETTERS_VS_OFFSET:
    .int 30746
SHADERS_GETTERS_VS_SIZE:
    .int 4809
SHADERS_LIGHT_DIRECTIONAL_FS_OFFSET:
    .int 35555
SHADERS_LIGHT_DIRECTIONAL_FS_SIZE:
    .int 2300
SHADERS_LIGHT_INDIRECT_FS_OFFSET:
    .int 37855
SHADERS_LIGHT_INDIRECT_FS_SIZE:
    .int 16730
SHADERS_LIGHT_REFLECTIONS_FS_OFFSET:
    .int 54585
SHADERS_LIGHT_REFLECTIONS_FS_SIZE:
    .int 4710
SHADERS_LIGHT_PUNCTUAL_FS_OFFSET:
    .int 59295
SHADERS_LIGHT_PUNCTUAL_FS_SIZE:
    .int 5676
SHADERS_MAIN_CS_OFFSET:
    .int 64971
SHADERS_MAIN_CS_SIZE:
    .int 28
SHADERS_MAIN_FS_OFFSET:
    .int 64999
SHADERS_MAIN_FS_SIZE:
    .int 1155
SHADERS_MAIN_VS_OFFSET:
    .int 66154
SHADERS_MAIN_VS_SIZE:
    .int 5189
SHADERS_MATERIAL_INPUTS_FS_OFFSET:
    .int 71343
SHADERS_MATERIAL_INPUTS_FS_SIZE:
    .int 4741
SHADERS_MATERIAL_INPUTS_VS_OFFSET:
    .int 76084
SHADERS_MATERIAL_INPUTS_VS_SIZE:
    .int 1723
SHADERS_POST_PROCESS_FS_OFFSET:
    .int 77807
SHADERS_POST_PROCESS_FS_SIZE:
    .int 1853
SHADERS_POST_PROCESS_VS_OFFSET:
    .int 79660
SHADERS_POST_PROCESS_VS_SIZE:
    .int 720
SHADERS_POST_PROCESS_GETTERS_VS_OFFSET:
    .int 80380
SHADERS_POST_PROCESS_GETTERS_VS_SIZE:
    .int 120
SHADERS_POST_PROCESS_INPUTS_FS_OFFSET:
    .int 80500
SHADERS_POST_PROCESS_INPUTS_FS_SIZE:
    .int 675
SHADERS_POST_PROCESS_INPUTS_VS_OFFSET:
    .int 81175
SHADERS_POST_PROCESS_INPUTS_VS_SIZE:
    .int 672
SHADERS_SHADING_LIT_FS_OFFSET:
    .int 81847
SHADERS_SHADING_LIT_FS_SIZE:
    .int 7698
SHADERS_SHADING_LIT_CUSTOM_FS_OFFSET:
    .int 89545
SHADERS_SHADING_LIT_CUSTOM_FS_SIZE:
    .int 641
SHADERS_SHADING_MODEL_CLOTH_FS_OFFSET:
    .int 90186
SHADERS_SHADING_MODEL_CLOTH_FS_SIZE:
    .int 947
SHADERS_SHADING_MODEL_STANDARD_FS_OFFSET:
    .int 91133
SHADERS_SHADING_MODEL_STANDARD_FS_SIZE:
    .int 3484
SHADERS_SHADING_MODEL_SUBSURFACE_FS_OFFSET:
    .int 94617
SHADERS_SHADING_MODEL_SUBSURFACE_FS_SIZE:
    .int 1056
SHADERS_SHADING_PARAMETERS_FS_OFFSET:
    .int 95673
SHADERS_SHADING_PARAMETERS_FS_SIZE:
    .int 1644
SHADERS_SHADING_REFLECTIONS_FS_OFFSET:
    .int 97317
SHADERS_SHADING_REFLECTIONS_FS_SIZE:
    .int 390
SHADERS_SHADING_UNLIT_FS_OFFSET:
    .int 97707
SHADERS_SHADING_UNLIT_FS_SIZE:
    .int 1578
SHADERS_SHADOWING_FS_OFFSET:
    .int 99285
SHADERS_SHADOWING_FS_SIZE:
    .int 16257
SHADERS_VARYINGS_GLSL_OFFSET:
    .int 115542
SHADERS_VARYINGS_GLSL_SIZE:
    .int 875
SHADERS_VIGNETTE_FS_OFFSET:
    .int 116417
SHADERS_VIGNETTE_FS_SIZE:
    .int 465

