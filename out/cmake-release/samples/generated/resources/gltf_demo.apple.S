    .global _GLTF_DEMO_DAMAGEDHELMET_OFFSET;
    .global _GLTF_DEMO_DAMAGEDHELMET_SIZE;
    .global _GLTF_DEMO_GROUNDSHADOW_OFFSET;
    .global _GLTF_DEMO_GROUNDSHADOW_SIZE;
    .global _GLTF_DEMO_OVERDRAW_OFFSET;
    .global _GLTF_DEMO_OVERDRAW_SIZE;

    .global _GLTF_DEMO_PACKAGE
    .section __TEXT,__const
_GLTF_DEMO_PACKAGE:
    .incbin "gltf_demo.bin"
_GLTF_DEMO_DAMAGEDHELMET_OFFSET:
    .int 0
_GLTF_DEMO_DAMAGEDHELMET_SIZE:
    .int 3773916
_GLTF_DEMO_GROUNDSHADOW_OFFSET:
    .int 3773916
_GLTF_DEMO_GROUNDSHADOW_SIZE:
    .int 204752
_GLTF_DEMO_OVERDRAW_OFFSET:
    .int 3978668
_GLTF_DEMO_OVERDRAW_SIZE:
    .int 17530

