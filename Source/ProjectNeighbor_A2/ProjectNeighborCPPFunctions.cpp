#include "ProjectNeighborCPPFunctions.h"

#if WITH_EDITOR
#include "Engine/BlueprintGeneratedClass.h"
#include "Kismet2/BlueprintEditorUtils.h"
#include "Editor.h"
#endif

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

void UProjectNeighborCPPFunctions::SetEditorIconForActorClass(UObject* ActorOrClass, UTexture2D* NewIcon)
{
	#if WITH_EDITOR
	if (!ActorOrClass || !NewIcon) return;

		UClass* TargetClass = nullptr;
		if (AActor* Actor = Cast<AActor>(ActorOrClass))
		{
			TargetClass = Actor->GetClass();
		}
		else if (UClass* Class = Cast<UClass>(ActorOrClass))
		{
			TargetClass = Class;
		}
		if (!TargetClass)return;

		if (UBlueprintGeneratedClass* BPClass = Cast<UBlueprintGeneratedClass>(TargetClass))
		{
			if (UBlueprint* BP = Cast<UBlueprint>(BPClass->ClassGeneratedBy))
			{
				
			}
		}

	#endif
}



void UProjectNeighborCPPFunctions::DisplayMessageBox(const FString& title, const FString& text)
{
	#if PLATFORM_WINDOWS
		MessageBox(nullptr, *text, *title, MB_OK);
	#endif
}

bool UProjectNeighborCPPFunctions::IsClassOrChild(UObject* obj, UClass* targetClass)
{
	if (!obj || !targetClass) { return false; }
	if (obj->IsA(targetClass)) { return true; }
	return false;
}