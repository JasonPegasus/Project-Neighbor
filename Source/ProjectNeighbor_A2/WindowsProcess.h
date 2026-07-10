// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/NoExportTypes.h"
#include "WindowsProcess.generated.h"

/**
 * 
 */
UCLASS(BlueprintType)
class PROJECTNEIGHBOR_A2_API UWindowsProcess : public UObject
{
	GENERATED_BODY()

public:

	UPROPERTY(BlueprintReadOnly) int32 PID;

	UPROPERTY(BlueprintReadOnly) FString Name;

	void Initialize(
		int32 InPID,
		const FString& InName
	);

	UFUNCTION(BlueprintCallable, meta = (Category = "Windows", DisplayName = "Kill")) void Kill();
	UFUNCTION(BlueprintCallable, meta = (Category = "Windows", DisplayName = "Set Suspend")) void SetSuspended(const bool suspend);
};
