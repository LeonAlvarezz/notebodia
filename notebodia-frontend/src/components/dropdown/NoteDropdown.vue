<template>
    <NDropdown :options="options" @select="handleSelect" trigger="click">
        <NButton
            @click.stop
            quaternary
            class="!text-color/70"
            :style="{
                padding: '10px',
                height: '16px',
                width: '16px',
            }"
        >
            <template #icon>
                <NIcon>
                    <IconMenu />
                </NIcon>
            </template>
        </NButton>
    </NDropdown>
</template>

<script setup lang="ts">
import { NDropdown, NIcon, useDialog, type MenuOption } from 'naive-ui'
import { IconEdit, IconTrash, IconMenu } from '@tabler/icons-vue'
import { renderIcon } from '@/util/render'
import type { Note } from '@/types/note.type'
import { deleteNote } from '@/api/note'
import { toast } from '@/composables/toast'
import { inject } from 'vue'
const dialog = useDialog()
const onView: (note: Note) => void = inject('onView', () => {})

const options: MenuOption[] = [
    {
        label: 'Edit',
        key: 'edit',
        icon: renderIcon(IconEdit), // Use a valid icon
    },
    {
        label: 'Delete',
        key: 'delete',
        icon: renderIcon(IconTrash, '!text-red-400'), // Use a valid icon
        props: {
            class: '!text-red-400',
        },
    },
]

const props = defineProps<{
    note: Note
}>()
const emit = defineEmits<{
    (e: 'revalidate'): void
}>()
const onDelete = async () => {
    const { error } = await deleteNote(props.note.id)
    if (error) {
        toast(error.message, {
            type: 'error',
        })
    }
    toast('Successfully Delete Note', {
        type: 'success',
    })
    emit('revalidate')
}
const handleSelect = (value: string) => {
    switch (value) {
        case 'delete':
            dialog.error({
                title: 'Confirm',
                content: 'Are you sure?',
                positiveText: 'Sure',
                negativeText: 'Not Sure',
                draggable: true,
                onPositiveClick: () => {
                    onDelete()
                },
            })
            break
        case 'edit':
            onView(props.note)
            break
    }
}
</script>
<style scoped></style>
