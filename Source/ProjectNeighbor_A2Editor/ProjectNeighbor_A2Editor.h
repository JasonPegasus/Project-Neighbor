#pragma once

#include "Modules/ModuleManager.h"

class FProjectNeighbor_A2EditorModule : public IModuleInterface
{
public:
    virtual void StartupModule() override;
    virtual void ShutdownModule() override;

private:
    TSharedPtr<class FSlateStyleSet> StyleSet;
};