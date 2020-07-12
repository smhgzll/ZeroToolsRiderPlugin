package com.jetbrains.rider.plugins.zerotoolspluginnet

import com.jetbrains.rider.settings.simple.SimpleOptionsPage

class SampleOptionsPage : SimpleOptionsPage("Sample Options", "SampleOptionsPage") {
    override fun getId(): String {
        return "SampleOptionsPage"
    }
}