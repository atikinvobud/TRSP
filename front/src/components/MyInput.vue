<script setup lang="ts">
const props = defineProps({
    modelValue: String,
    inputType: {
        type: String,

    },
    placeholder: {
        type: String,
        default: '',
    },
    isPassword: {
        type: Boolean,
        default: false,
  },
})

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void
}>()
</script>

<template>
<input
    :value="modelValue"
    @input="(e) => emit('update:modelValue', (e.target as HTMLInputElement).value)"
    :placeholder='placeholder'
    :type="isPassword ? 'password' : inputType"
    class="border rounded p-1 w-full">
</template>