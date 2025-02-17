<template>
    <NModal
        size="huge"
        preset="dialog"
        :show="props.show"
        :show-icon="false"
        transform-origin="center"
        title="New Note"
        :style="{ minWidth: '500px' }"
        positive-text="Save"
        :positive-button-props="{ loading }"
        @esc="closeModal"
        @close="closeModal"
        @mask-click="closeModal"
        @positive-click="onSubmit"
    >
        >
    </NModal>
</template>

<script setup lang="ts">
import { ref } from 'vue'
const props = defineProps<{
    show: boolean
}>()
const emit = defineEmits<{
    (e: 'update:show', value: boolean): void
}>()
const loading = ref(false)
const onSubmit = async () => {
    console.log('Submit')
}
const closeModal = () => {
    emit('update:show', false)
}
</script>

<style scoped></style>
