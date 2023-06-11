#ifndef VKSHADERS_H_
#define VKSHADERS_H_

#include <stdint.h>

extern "C" {
    extern const uint8_t VKSHADERS_PACKAGE[];
    extern int VKSHADERS_BLITDEPTHVS_OFFSET;
    extern int VKSHADERS_BLITDEPTHVS_SIZE;
    extern int VKSHADERS_BLITDEPTHFS_OFFSET;
    extern int VKSHADERS_BLITDEPTHFS_SIZE;
}
#define VKSHADERS_BLITDEPTHVS_DATA (VKSHADERS_PACKAGE + VKSHADERS_BLITDEPTHVS_OFFSET)
#define VKSHADERS_BLITDEPTHFS_DATA (VKSHADERS_PACKAGE + VKSHADERS_BLITDEPTHFS_OFFSET)

#endif