    .global FILAMENTAPP_AIDEFAULTMAT_OFFSET;
    .global FILAMENTAPP_AIDEFAULTMAT_SIZE;
    .global FILAMENTAPP_AIDEFAULTTRANS_OFFSET;
    .global FILAMENTAPP_AIDEFAULTTRANS_SIZE;
    .global FILAMENTAPP_DEPTHVISUALIZER_OFFSET;
    .global FILAMENTAPP_DEPTHVISUALIZER_SIZE;
    .global FILAMENTAPP_TRANSPARENTCOLOR_OFFSET;
    .global FILAMENTAPP_TRANSPARENTCOLOR_SIZE;

    .global FILAMENTAPP_PACKAGE
    .section .rodata
FILAMENTAPP_PACKAGE:
    .incbin "filamentapp.bin"
FILAMENTAPP_AIDEFAULTMAT_OFFSET:
    .int 0
FILAMENTAPP_AIDEFAULTMAT_SIZE:
    .int 938572
FILAMENTAPP_AIDEFAULTTRANS_OFFSET:
    .int 938572
FILAMENTAPP_AIDEFAULTTRANS_SIZE:
    .int 752388
FILAMENTAPP_DEPTHVISUALIZER_OFFSET:
    .int 1690960
FILAMENTAPP_DEPTHVISUALIZER_SIZE:
    .int 33526
FILAMENTAPP_TRANSPARENTCOLOR_OFFSET:
    .int 1724486
FILAMENTAPP_TRANSPARENTCOLOR_SIZE:
    .int 33189

