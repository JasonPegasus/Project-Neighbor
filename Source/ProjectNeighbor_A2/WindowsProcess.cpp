// Fill out your copyright notice in the Description page of Project Settings.


#include "WindowsProcess.h"

#if PLATFORM_WINDOWS

#include "Windows/AllowWindowsPlatformTypes.h"
#include "Windows/PreWindowsApi.h"

#include <winuser.h>

#include "Windows/PostWindowsApi.h"
#include "Windows/HideWindowsPlatformTypes.h"

#include <tlhelp32.h>
#include <processthreadsapi.h>
#include <handleapi.h>

#endif

#include "WindowsProcessLibrary.h"

void UWindowsProcess::Initialize(int32 InPID, const FString& InName) { PID = InPID; Name = InName; }

void UWindowsProcess::Kill() { UWindowsProcessLibrary::KillByPID(PID); }
void UWindowsProcess::SetSuspended(const bool suspend) { UWindowsProcessLibrary::SetSuspendByPID(PID, suspend); }