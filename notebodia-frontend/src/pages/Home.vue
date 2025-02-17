<template>
    <NFloatButton
        shape="circle"
        class="!bg-primary hover:!bg-primary/80 fixed right-0 bottom-4 z-20 mx-7 md:mx-18 lg:mx-30 xl:mx-80"
        @click="showModal = true"
    >
        <NIcon>
            <IconPlus />
        </NIcon>
    </NFloatButton>
    <div
        class="bg-background sticky top-0 left-0 z-20 flex flex-col gap-6 py-4"
    >
        <div class="flex items-center justify-between">
            <h1 class="text-3xl">Note</h1>
            <ProfileDropdown />
        </div>
        <div class="flex w-full items-center">
            <NInput
                v-model:value="filters.keyword"
                placeholder="Search by title or content"
                size="large"
                round
                class="px-2"
                @input="debounceSearch('keyword')"
            >
                <template #suffix>
                    <NIcon :component="IconSearch" />
                </template>
            </NInput>
            <!-- <NoteFilterDropdown />
            <NButton
                @click.stop
                quaternary
                class="!text-color/70"
                :style="{
                    padding: '10px',
                }"
            >
                <template #icon>
                    <NIcon>
                        <IconSortAscending />
                    </NIcon>
                </template>
            </NButton> -->
        </div>
    </div>
    <div
        v-if="loading"
        class="flex min-h-[calc(100svh-170px)] w-full items-center justify-center"
    >
        <NSpin size="medium" />
    </div>
    <div
        v-else-if="!loading && notes.length <= 0"
        class="flex min-h-[calc(100svh-170px)] w-full items-center justify-center"
    >
        No Note Yet
    </div>
    <div v-else class="grid grid-cols-2 gap-4 py-4">
        <NoteCard
            v-for="(note, index) in notes"
            :key="index"
            :note="note"
            @on-view="onView"
            @revalidate="fetchNotes"
        />
    </div>

    <NoteModal
        v-model:show="showModal"
        @revalidate="fetchNotes"
        :note="selectedNote"
    />
</template>

<script setup lang="ts">
import { getUserNote } from '@/api/note'
import NoteModal from '@/components/modal/NoteModal.vue'
import NoteCard from '@/components/note/NoteCard.vue'
import { toast } from '@/composables/toast'
import type { Note, NoteFilter } from '@/types/note.type'
import { IconPlus, IconSearch } from '@tabler/icons-vue'
import { useDebounceFn } from '@vueuse/core'
import { onMounted, provide, reactive, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const showModal = ref(false)
const loading = ref(false)
const notes = ref<Note[]>([])
const selectedNote = ref<Note>()
const router = useRouter()
const route = useRoute()
const filters = reactive<NoteFilter>({
    keyword: route.query.keyword?.toString(),
})
const onView = (note: Note) => {
    selectedNote.value = note
    showModal.value = true
}
async function fetchNotes() {
    loading.value = true
    try {
        const { data, error } = await getUserNote(filters)
        if (error) {
            toast(`Failed To Fetch Notes: ${error.message}`, {
                type: 'error',
            })
        }
        notes.value = data!
    } catch (error) {
    } finally {
        loading.value = false
    }
}
onMounted(async () => {
    await fetchNotes()
})

const debounceSearch = useDebounceFn((key: keyof NoteFilter) => {
    if (filters[key]) {
        const query = { ...route.query }
        console.log('query:', query)
        router.push({
            query: { ...route.query, [key]: filters[key] },
        })
    } else {
        // If the keyword is empty, remove it from the query
        const query = { ...route.query }
        console.log('query:', query)
        delete query[key]
        router.push({ query })
    }
}, 1000)

watch(route, async () => {
    await fetchNotes()
})

provide('showModal', showModal)
provide('selectedNote', selectedNote)
</script>

<style scoped></style>
