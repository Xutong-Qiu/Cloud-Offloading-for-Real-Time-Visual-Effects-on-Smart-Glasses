    .global FILAGUI_RESOURCES_UIBLIT_OFFSET;
    .global FILAGUI_RESOURCES_UIBLIT_SIZE;

    .global FILAGUI_RESOURCES_PACKAGE
    .section .rodata
FILAGUI_RESOURCES_PACKAGE:
    .incbin "filagui_resources.bin"
FILAGUI_RESOURCES_UIBLIT_OFFSET:
    .int 0
FILAGUI_RESOURCES_UIBLIT_SIZE:
    .int 23614

