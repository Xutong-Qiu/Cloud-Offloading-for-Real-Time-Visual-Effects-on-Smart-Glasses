#ifndef MONKEY_H_
#define MONKEY_H_

#include <stdint.h>

extern "C" {
    extern const uint8_t MONKEY_PACKAGE[];
    extern int MONKEY_SUZANNE_OFFSET;
    extern int MONKEY_SUZANNE_SIZE;
    extern int MONKEY_ALBEDO_OFFSET;
    extern int MONKEY_ALBEDO_SIZE;
    extern int MONKEY_ROUGHNESS_OFFSET;
    extern int MONKEY_ROUGHNESS_SIZE;
    extern int MONKEY_METALLIC_OFFSET;
    extern int MONKEY_METALLIC_SIZE;
    extern int MONKEY_AO_OFFSET;
    extern int MONKEY_AO_SIZE;
    extern int MONKEY_NORMAL_OFFSET;
    extern int MONKEY_NORMAL_SIZE;
}
#define MONKEY_SUZANNE_DATA (MONKEY_PACKAGE + MONKEY_SUZANNE_OFFSET)
#define MONKEY_ALBEDO_DATA (MONKEY_PACKAGE + MONKEY_ALBEDO_OFFSET)
#define MONKEY_ROUGHNESS_DATA (MONKEY_PACKAGE + MONKEY_ROUGHNESS_OFFSET)
#define MONKEY_METALLIC_DATA (MONKEY_PACKAGE + MONKEY_METALLIC_OFFSET)
#define MONKEY_AO_DATA (MONKEY_PACKAGE + MONKEY_AO_OFFSET)
#define MONKEY_NORMAL_DATA (MONKEY_PACKAGE + MONKEY_NORMAL_OFFSET)

#endif
