<template>
    <NCard
        :title="note.title"
        class="3xl:max-w-xl hover:cursor-pointer"
        @click="emit('on-view', note)"
    >
        <template #header-extra>
            <NoteDropdown :note="note" @revalidate="emit('revalidate')" />
        </template>
        <p class="line-clamp-3">
            {{
                props.note.content && props.note.content.length > 0
                    ? props.note.content
                    : '-'
            }}
        </p>
        <template #footer>
            <div class="flex justify-end">
                <p class="text-subtitle text-xxs">
                    {{ formatDateTime(note.updatedAt) }}
                    <!-- {{ note.updatedAt }} -->
                </p>
            </div>
        </template>
    </NCard>
</template>

<script setup lang="ts">
import NoteDropdown from '@/components/dropdown/NoteDropdown.vue'
import type { Note } from '@/types/note.type'
import { formatDateTime } from '@/util/date'
const props = defineProps<{
    note: Note
}>()
const emit = defineEmits<{
    (e: 'on-view', value: Note): void
    (e: 'revalidate'): void
}>()
</script>

<style scoped></style>
