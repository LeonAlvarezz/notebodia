<template>
    <NDropdown
        :options="options"
        trigger="click"
        :value="selectedValue"
        @select="handleSelect"
    >
        <NButton
            @click.stop
            quaternary
            class="!text-color/70"
            :style="{ padding: '10px' }"
        >
            <template #icon>
                <NIcon>
                    <IconFilter />
                </NIcon>
            </template>
        </NButton>
    </NDropdown>
</template>

<script setup lang="ts">
import { h, ref } from 'vue'
import { NRadio, type MenuOption } from 'naive-ui'
import { IconFilter } from '@tabler/icons-vue'

const selectedValue = ref('all')

const options: MenuOption[] = [
    {
        label: 'All Notes',
        key: 'all',
        renderOption: renderRadioOption,
    },
    {
        label: 'Published',
        key: 'published',
        renderOption: renderRadioOption,
    },
    {
        label: 'Unpublished',
        key: 'unpublished',
        renderOption: renderRadioOption,
    },
]

function renderRadioOption(option: MenuOption) {
    return h(NRadio, {
        checked: selectedValue.value === option.key,
        label: option.label as string,
        value: option.key,
    })
}

const handleSelect = (key: string) => {
    selectedValue.value = key
    // Emit the selected value to parent component
    emit('update:filter', key)
}

// Define emits
const emit = defineEmits(['update:filter'])
</script>

<style scoped></style>
