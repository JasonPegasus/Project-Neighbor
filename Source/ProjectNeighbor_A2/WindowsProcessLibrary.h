// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "WindowsProcessLibrary.generated.h"

/**
 * 
 */
UCLASS(BlueprintType)
class PROJECTNEIGHBOR_A2_API UWindowsProcessLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()

public:

	UFUNCTION(BlueprintCallable, meta = (Category = "Windows", DisplayName = "Get Process by Name"))
	static UWindowsProcess* GetProcessByName(const FString& name);


	UFUNCTION(BlueprintCallable, meta = (Category = "Windows", DisplayName = "Kill Process by PID"))
	static void KillByPID(const int32& PID);

	UFUNCTION(BlueprintCallable, meta = (Category = "Windows", DisplayName = "Suspend Process by PID"))
	static void SetSuspendByPID(const int32& PID, const bool& suspend);
};
