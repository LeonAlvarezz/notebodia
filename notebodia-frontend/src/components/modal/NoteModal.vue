<template>
    <NModal
        size="huge"
        preset="dialog"
        :show="props.show"
        :show-icon="false"
        transform-origin="center"
        title="New Note"
        :style="{ width: '600px' }"
        positive-text="Save"
        :positive-button-props="{ loading }"
        @esc="closeModal"
        @close="closeModal"
        @mask-click="closeModal"
        @positive-click="onSubmit"
    >
        <NForm ref="formRef" :model="payload" :rules="rules" class="my-6">
            <NFormItem path="title" label="Title" label-placement="top">
                <NInput placeholder="New Note" v-model:value="payload.title" />
            </NFormItem>
            <NFormItem label="Content" label-placement="top">
                <NInput
                    type="textarea"
                    placeholder="This is my new note"
                    v-model:value="payload.content"
                    :autosize="{
                        minRows: 3,
                    }"
                />
            </NFormItem>
        </NForm>
    </NModal>
</template>

<script setup lang="ts">
import { inject, ref, watch } from 'vue'
import type { CreateNewNote, Note } from '@/types/note.type'
import type { FormInst, FormItemRule, FormRules } from 'naive-ui'
import { createNote, updateNote } from '@/api/note'
import { toast } from '@/composables/toast'
const onClear: () => void = inject('onClear', () => {})
const props = defineProps<{
    show: boolean
    note?: Note
}>()
const emit = defineEmits<{
    (e: 'update:show', value: boolean): void
    (e: 'revalidate'): void
}>()

const defaultValue: CreateNewNote = {
    title: '',
    content: '',
}
const payload = ref<CreateNewNote>({ ...defaultValue })
const formRef = ref<FormInst | null>(null)
const rules: FormRules = {
    title: [
        {
            required: true,
            message: 'Title is required',
            trigger: 'blur',
        },
        {
            message: 'Title Cannot Be Longer than 100 characters',
            trigger: 'blur',
            validator(_: FormItemRule, value: string) {
                if (value.length > 100) {
                    return new Error(
                        'Value must not be more than 100 characters'
                    )
                }
                return true
            },
        },
    ],
}

const loading = ref(false)
const onSubmit = async () => {
    loading.value = true
    formRef.value?.validate(async (errors) => {
        try {
            if (errors) return
            const data: CreateNewNote = {
                title: payload.value.title.trim(),
                content: payload.value.content.trim(),
            }
            const { error: resError } = props.note
                ? await updateNote(props.note.id, data)
                : await createNote(data)
            if (resError) {
                toast(`Failed to create note: ${resError.message}`, {
                    type: 'error',
                })
            }
            toast('Successfully Create Note', {
                type: 'success',
            })
            emit('revalidate')
            closeModal()
        } catch (error: any) {
        } finally {
            loading.value = false
        }
    })
}
const closeModal = () => {
    onClear()
    payload.value = { ...defaultValue }

    emit('update:show', false)
}

//Watch
watch([props], () => {
    console.log('props.note:', props.note)
    if (props.note) {
        payload.value = {
            content: props.note.content ?? '',
            title: props.note.title ?? '',
        }
    }
})
</script>

<style scoped></style>
