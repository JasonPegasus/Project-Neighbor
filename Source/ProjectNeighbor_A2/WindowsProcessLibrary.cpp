#include "WindowsProcessLibrary.h"

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

#include "WindowsProcess.h"

UWindowsProcess* UWindowsProcessLibrary::GetProcessByName(const FString& name)
{
#if PLATFORM_WINDOWS
	HANDLE Snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

	if (Snapshot == INVALID_HANDLE_VALUE)                                                 
		return nullptr;

	PROCESSENTRY32 ProcessEntry;
	ProcessEntry.dwSize = sizeof(PROCESSENTRY32);               

	if (Process32First(Snapshot, &ProcessEntry))                           
	{
		do
		{
			FString Name = ProcessEntry.szExeFile;

			if (Name.Equals(name, ESearchCase::IgnoreCase))               
			{
				CloseHandle(Snapshot);
				UWindowsProcess* process = NewObject<UWindowsProcess>();
				process->Initialize(ProcessEntry.th32ProcessID, Name);
				return process;
			}

		} while (Process32Next(Snapshot, &ProcessEntry));     
	}

	CloseHandle(Snapshot);
#endif
	return nullptr;
}

void UWindowsProcessLibrary::KillByPID(const int32& PID)
{
	#if PLATFORM_WINDOWS

		HANDLE Process = OpenProcess(PROCESS_TERMINATE, false, PID);

		TerminateProcess(Process, 0);

		CloseHandle(Process);
	#endif
}

using FNtSuspendProcess = LONG(NTAPI*)(HANDLE);
using FNtResumeProcess = LONG(NTAPI*)(HANDLE);
HMODULE ntdll = GetModuleHandle(TEXT("ntdll.dll"));
FNtSuspendProcess SuspendProcess = nullptr;
FNtSuspendProcess ResumeProcess = nullptr;


void SetSuspendByHandle(HANDLE ProcessHandle, const bool& suspend) {
	if (!SuspendProcess || !ResumeProcess){
		SuspendProcess = (FNtSuspendProcess)GetProcAddress(ntdll, "NtSuspendProcess");
		ResumeProcess  = (FNtResumeProcess )GetProcAddress(ntdll, "NtResumeProcess" );
	}
	if (suspend) { SuspendProcess(ProcessHandle); return; }
	ResumeProcess(ProcessHandle);
}

void UWindowsProcessLibrary::SetSuspendByPID(const int32& PID, const bool& suspend)
{
	#if PLATFORM_WINDOWS
	HANDLE process = OpenProcess( PROCESS_ALL_ACCESS, false, PID );
	if (!process) return;
	SetSuspendByHandle(process, suspend);
	CloseHandle(process);
	#endif	
}