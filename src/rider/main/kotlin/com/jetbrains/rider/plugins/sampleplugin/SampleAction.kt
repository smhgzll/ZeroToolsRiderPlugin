package com.jetbrains.rider.plugins.zerotoolspluginnet

import com.intellij.openapi.actionSystem.AnActionEvent
import com.jetbrains.rider.actions.base.RiderAnAction
import com.jetbrains.rider.plugins.sampleplugin.MenuIcons


class GenerateEntity : RiderAnAction("GenerateEntityAction",
        "Generate Entity", "", MenuIcons.ICON_GENERATE) {
    override fun actionPerformed(e: AnActionEvent) {
        super.actionPerformed(e)
    }

}

class RegenerateEntity : RiderAnAction("RegenerateEntityAction",
        "Regenerate Entity", "", MenuIcons.ICON_REGENERATE) {
    override fun actionPerformed(e: AnActionEvent) {
        super.actionPerformed(e)
    }

}

class LoadFromDatabase : RiderAnAction("LoadEntityFromDatabaseAction",
        "Load From Database", "", MenuIcons.ICON_FROM_DATABASE) {
    override fun actionPerformed(e: AnActionEvent) {
        super.actionPerformed(e)
    }

}

class Info : RiderAnAction("InfoAction",
        "About Power Tools", "", MenuIcons.ICON_INFO) {
    override fun actionPerformed(e: AnActionEvent) {
        super.actionPerformed(e)
    }

}