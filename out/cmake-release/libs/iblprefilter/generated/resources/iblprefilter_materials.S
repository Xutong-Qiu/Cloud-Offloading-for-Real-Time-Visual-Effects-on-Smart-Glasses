    .global IBLPREFILTER_MATERIALS_EQUIRECTTOCUBE_OFFSET;
    .global IBLPREFILTER_MATERIALS_EQUIRECTTOCUBE_SIZE;
    .global IBLPREFILTER_MATERIALS_GENERATEKERNEL_OFFSET;
    .global IBLPREFILTER_MATERIALS_GENERATEKERNEL_SIZE;
    .global IBLPREFILTER_MATERIALS_IBLPREFILTER_OFFSET;
    .global IBLPREFILTER_MATERIALS_IBLPREFILTER_SIZE;

    .global IBLPREFILTER_MATERIALS_PACKAGE
    .section .rodata
IBLPREFILTER_MATERIALS_PACKAGE:
    .incbin "iblprefilter_materials.bin"
IBLPREFILTER_MATERIALS_EQUIRECTTOCUBE_OFFSET:
    .int 0
IBLPREFILTER_MATERIALS_EQUIRECTTOCUBE_SIZE:
    .int 7248
IBLPREFILTER_MATERIALS_GENERATEKERNEL_OFFSET:
    .int 7248
IBLPREFILTER_MATERIALS_GENERATEKERNEL_SIZE:
    .int 6690
IBLPREFILTER_MATERIALS_IBLPREFILTER_OFFSET:
    .int 13938
IBLPREFILTER_MATERIALS_IBLPREFILTER_SIZE:
    .int 13662

