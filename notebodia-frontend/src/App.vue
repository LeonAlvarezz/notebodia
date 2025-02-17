<script setup lang="ts">
import './style.css'
import { darkTheme } from 'naive-ui'
import { themeOverrides } from '@/theme/themeOverrides'
</script>

<template>
    <NConfigProvider :theme="darkTheme" :theme-overrides="themeOverrides">
        <NDialogProvider>
            <NMessageProvider>
                <NNotificationProvider>
                    <div class="px-8 py-4 md:px-20 lg:px-40 xl:px-80">
                        <RouterView />
                    </div>
                </NNotificationProvider>
            </NMessageProvider>
        </NDialogProvider>
    </NConfigProvider>
</template>

<style scoped></style>
