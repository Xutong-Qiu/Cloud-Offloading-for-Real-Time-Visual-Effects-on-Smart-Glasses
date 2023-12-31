#ifndef MATERIALS_H_
#define MATERIALS_H_

#include <stdint.h>

extern "C" {
    extern const uint8_t MATERIALS_PACKAGE[];
    extern int MATERIALS_FSR_EASU_OFFSET;
    extern int MATERIALS_FSR_EASU_SIZE;
    extern int MATERIALS_FSR_EASU_MOBILE_OFFSET;
    extern int MATERIALS_FSR_EASU_MOBILE_SIZE;
    extern int MATERIALS_FSR_EASU_MOBILEF_OFFSET;
    extern int MATERIALS_FSR_EASU_MOBILEF_SIZE;
    extern int MATERIALS_FSR_RCAS_OFFSET;
    extern int MATERIALS_FSR_RCAS_SIZE;
    extern int MATERIALS_COLORGRADING_OFFSET;
    extern int MATERIALS_COLORGRADING_SIZE;
    extern int MATERIALS_COLORGRADINGASSUBPASS_OFFSET;
    extern int MATERIALS_COLORGRADINGASSUBPASS_SIZE;
    extern int MATERIALS_CUSTOMRESOLVEASSUBPASS_OFFSET;
    extern int MATERIALS_CUSTOMRESOLVEASSUBPASS_SIZE;
    extern int MATERIALS_DEBUGSHADOWCASCADES_OFFSET;
    extern int MATERIALS_DEBUGSHADOWCASCADES_SIZE;
    extern int MATERIALS_DEFAULTMATERIAL_OFFSET;
    extern int MATERIALS_DEFAULTMATERIAL_SIZE;
    extern int MATERIALS_DOF_OFFSET;
    extern int MATERIALS_DOF_SIZE;
    extern int MATERIALS_DOFCOC_OFFSET;
    extern int MATERIALS_DOFCOC_SIZE;
    extern int MATERIALS_DOFDOWNSAMPLE_OFFSET;
    extern int MATERIALS_DOFDOWNSAMPLE_SIZE;
    extern int MATERIALS_DOFCOMBINE_OFFSET;
    extern int MATERIALS_DOFCOMBINE_SIZE;
    extern int MATERIALS_DOFTILES_OFFSET;
    extern int MATERIALS_DOFTILES_SIZE;
    extern int MATERIALS_DOFTILESSWIZZLE_OFFSET;
    extern int MATERIALS_DOFTILESSWIZZLE_SIZE;
    extern int MATERIALS_DOFDILATE_OFFSET;
    extern int MATERIALS_DOFDILATE_SIZE;
    extern int MATERIALS_DOFMIPMAP_OFFSET;
    extern int MATERIALS_DOFMIPMAP_SIZE;
    extern int MATERIALS_DOFMEDIAN_OFFSET;
    extern int MATERIALS_DOFMEDIAN_SIZE;
    extern int MATERIALS_FLARE_OFFSET;
    extern int MATERIALS_FLARE_SIZE;
    extern int MATERIALS_BLITLOW_OFFSET;
    extern int MATERIALS_BLITLOW_SIZE;
    extern int MATERIALS_BLOOMDOWNSAMPLE_OFFSET;
    extern int MATERIALS_BLOOMDOWNSAMPLE_SIZE;
    extern int MATERIALS_BLOOMUPSAMPLE_OFFSET;
    extern int MATERIALS_BLOOMUPSAMPLE_SIZE;
    extern int MATERIALS_BILATERALBLUR_OFFSET;
    extern int MATERIALS_BILATERALBLUR_SIZE;
    extern int MATERIALS_BILATERALBLURBENTNORMALS_OFFSET;
    extern int MATERIALS_BILATERALBLURBENTNORMALS_SIZE;
    extern int MATERIALS_MIPMAPDEPTH_OFFSET;
    extern int MATERIALS_MIPMAPDEPTH_SIZE;
    extern int MATERIALS_SKYBOX_OFFSET;
    extern int MATERIALS_SKYBOX_SIZE;
    extern int MATERIALS_SAO_OFFSET;
    extern int MATERIALS_SAO_SIZE;
    extern int MATERIALS_SAOBENTNORMALS_OFFSET;
    extern int MATERIALS_SAOBENTNORMALS_SIZE;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR1_OFFSET;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR1_SIZE;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR2_OFFSET;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR2_SIZE;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR3_OFFSET;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR3_SIZE;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR4_OFFSET;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR4_SIZE;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR1L_OFFSET;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR1L_SIZE;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR2L_OFFSET;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR2L_SIZE;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR3L_OFFSET;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR3L_SIZE;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR4L_OFFSET;
    extern int MATERIALS_SEPARABLEGAUSSIANBLUR4L_SIZE;
    extern int MATERIALS_FXAA_OFFSET;
    extern int MATERIALS_FXAA_SIZE;
    extern int MATERIALS_TAA_OFFSET;
    extern int MATERIALS_TAA_SIZE;
    extern int MATERIALS_VSMMIPMAP_OFFSET;
    extern int MATERIALS_VSMMIPMAP_SIZE;
}
#define MATERIALS_FSR_EASU_DATA (MATERIALS_PACKAGE + MATERIALS_FSR_EASU_OFFSET)
#define MATERIALS_FSR_EASU_MOBILE_DATA (MATERIALS_PACKAGE + MATERIALS_FSR_EASU_MOBILE_OFFSET)
#define MATERIALS_FSR_EASU_MOBILEF_DATA (MATERIALS_PACKAGE + MATERIALS_FSR_EASU_MOBILEF_OFFSET)
#define MATERIALS_FSR_RCAS_DATA (MATERIALS_PACKAGE + MATERIALS_FSR_RCAS_OFFSET)
#define MATERIALS_COLORGRADING_DATA (MATERIALS_PACKAGE + MATERIALS_COLORGRADING_OFFSET)
#define MATERIALS_COLORGRADINGASSUBPASS_DATA (MATERIALS_PACKAGE + MATERIALS_COLORGRADINGASSUBPASS_OFFSET)
#define MATERIALS_CUSTOMRESOLVEASSUBPASS_DATA (MATERIALS_PACKAGE + MATERIALS_CUSTOMRESOLVEASSUBPASS_OFFSET)
#define MATERIALS_DEBUGSHADOWCASCADES_DATA (MATERIALS_PACKAGE + MATERIALS_DEBUGSHADOWCASCADES_OFFSET)
#define MATERIALS_DEFAULTMATERIAL_DATA (MATERIALS_PACKAGE + MATERIALS_DEFAULTMATERIAL_OFFSET)
#define MATERIALS_DOF_DATA (MATERIALS_PACKAGE + MATERIALS_DOF_OFFSET)
#define MATERIALS_DOFCOC_DATA (MATERIALS_PACKAGE + MATERIALS_DOFCOC_OFFSET)
#define MATERIALS_DOFDOWNSAMPLE_DATA (MATERIALS_PACKAGE + MATERIALS_DOFDOWNSAMPLE_OFFSET)
#define MATERIALS_DOFCOMBINE_DATA (MATERIALS_PACKAGE + MATERIALS_DOFCOMBINE_OFFSET)
#define MATERIALS_DOFTILES_DATA (MATERIALS_PACKAGE + MATERIALS_DOFTILES_OFFSET)
#define MATERIALS_DOFTILESSWIZZLE_DATA (MATERIALS_PACKAGE + MATERIALS_DOFTILESSWIZZLE_OFFSET)
#define MATERIALS_DOFDILATE_DATA (MATERIALS_PACKAGE + MATERIALS_DOFDILATE_OFFSET)
#define MATERIALS_DOFMIPMAP_DATA (MATERIALS_PACKAGE + MATERIALS_DOFMIPMAP_OFFSET)
#define MATERIALS_DOFMEDIAN_DATA (MATERIALS_PACKAGE + MATERIALS_DOFMEDIAN_OFFSET)
#define MATERIALS_FLARE_DATA (MATERIALS_PACKAGE + MATERIALS_FLARE_OFFSET)
#define MATERIALS_BLITLOW_DATA (MATERIALS_PACKAGE + MATERIALS_BLITLOW_OFFSET)
#define MATERIALS_BLOOMDOWNSAMPLE_DATA (MATERIALS_PACKAGE + MATERIALS_BLOOMDOWNSAMPLE_OFFSET)
#define MATERIALS_BLOOMUPSAMPLE_DATA (MATERIALS_PACKAGE + MATERIALS_BLOOMUPSAMPLE_OFFSET)
#define MATERIALS_BILATERALBLUR_DATA (MATERIALS_PACKAGE + MATERIALS_BILATERALBLUR_OFFSET)
#define MATERIALS_BILATERALBLURBENTNORMALS_DATA (MATERIALS_PACKAGE + MATERIALS_BILATERALBLURBENTNORMALS_OFFSET)
#define MATERIALS_MIPMAPDEPTH_DATA (MATERIALS_PACKAGE + MATERIALS_MIPMAPDEPTH_OFFSET)
#define MATERIALS_SKYBOX_DATA (MATERIALS_PACKAGE + MATERIALS_SKYBOX_OFFSET)
#define MATERIALS_SAO_DATA (MATERIALS_PACKAGE + MATERIALS_SAO_OFFSET)
#define MATERIALS_SAOBENTNORMALS_DATA (MATERIALS_PACKAGE + MATERIALS_SAOBENTNORMALS_OFFSET)
#define MATERIALS_SEPARABLEGAUSSIANBLUR1_DATA (MATERIALS_PACKAGE + MATERIALS_SEPARABLEGAUSSIANBLUR1_OFFSET)
#define MATERIALS_SEPARABLEGAUSSIANBLUR2_DATA (MATERIALS_PACKAGE + MATERIALS_SEPARABLEGAUSSIANBLUR2_OFFSET)
#define MATERIALS_SEPARABLEGAUSSIANBLUR3_DATA (MATERIALS_PACKAGE + MATERIALS_SEPARABLEGAUSSIANBLUR3_OFFSET)
#define MATERIALS_SEPARABLEGAUSSIANBLUR4_DATA (MATERIALS_PACKAGE + MATERIALS_SEPARABLEGAUSSIANBLUR4_OFFSET)
#define MATERIALS_SEPARABLEGAUSSIANBLUR1L_DATA (MATERIALS_PACKAGE + MATERIALS_SEPARABLEGAUSSIANBLUR1L_OFFSET)
#define MATERIALS_SEPARABLEGAUSSIANBLUR2L_DATA (MATERIALS_PACKAGE + MATERIALS_SEPARABLEGAUSSIANBLUR2L_OFFSET)
#define MATERIALS_SEPARABLEGAUSSIANBLUR3L_DATA (MATERIALS_PACKAGE + MATERIALS_SEPARABLEGAUSSIANBLUR3L_OFFSET)
#define MATERIALS_SEPARABLEGAUSSIANBLUR4L_DATA (MATERIALS_PACKAGE + MATERIALS_SEPARABLEGAUSSIANBLUR4L_OFFSET)
#define MATERIALS_FXAA_DATA (MATERIALS_PACKAGE + MATERIALS_FXAA_OFFSET)
#define MATERIALS_TAA_DATA (MATERIALS_PACKAGE + MATERIALS_TAA_OFFSET)
#define MATERIALS_VSMMIPMAP_DATA (MATERIALS_PACKAGE + MATERIALS_VSMMIPMAP_OFFSET)

#endif
