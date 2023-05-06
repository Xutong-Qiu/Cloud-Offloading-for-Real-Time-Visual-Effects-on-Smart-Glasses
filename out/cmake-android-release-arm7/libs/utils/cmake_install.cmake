# Install script for directory: /Users/wei/Desktop/filament-PowerVR/libs/utils

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "/Users/wei/Desktop/filament-PowerVR/out/android-release/filament")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Release")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Install shared libraries without execute permission?
if(NOT DEFINED CMAKE_INSTALL_SO_NO_EXE)
  set(CMAKE_INSTALL_SO_NO_EXE "0")
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "TRUE")
endif()

# Set default install directory permissions.
if(NOT DEFINED CMAKE_OBJDUMP)
  set(CMAKE_OBJDUMP "/Users/wei/Library/Android/sdk/ndk/25.1.8937393/toolchains/llvm/prebuilt/darwin-x86_64/bin/llvm-objdump")
endif()

if(CMAKE_INSTALL_COMPONENT STREQUAL "Unspecified" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/armeabi-v7a" TYPE STATIC_LIBRARY FILES "/Users/wei/Desktop/filament-PowerVR/out/cmake-android-release-arm7/libs/utils/libutils.a")
endif()

if(CMAKE_INSTALL_COMPONENT STREQUAL "Unspecified" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/utils" TYPE FILE FILES
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/algorithm.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/bitset.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/CallStack.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/debug.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/Allocator.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/BitmaskEnum.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/compiler.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/compressed_pair.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/CString.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/Entity.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/EntityInstance.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/EntityManager.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/FixedCapacityVector.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/Invocable.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/Log.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/memalign.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/Mutex.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/NameComponentManager.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/ostream.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/Panic.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/Path.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/PrivateImplementation.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/PrivateImplementation-impl.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/SingleInstanceComponentManager.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/Slice.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/SpinLock.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/StructureOfArrays.h"
    "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/unwindows.h"
    )
endif()

if(CMAKE_INSTALL_COMPONENT STREQUAL "Unspecified" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/utils/linux" TYPE FILE FILES "/Users/wei/Desktop/filament-PowerVR/libs/utils/include/utils/linux/Mutex.h")
endif()

