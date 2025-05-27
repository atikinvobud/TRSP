<script setup lang="ts">
  import MyButton from './MyButton.vue'

  const props = defineProps({
    type: {
      type: String,
      required: true,
    },
    lotName: {
      type: String,
      required: true,
    },
    userName: {
      type: String,
      required: true,
    },
    bet: {
      type: Number,
      required: true,
    },
  })
</script>

<template>
  <div class="flex flex-col gap-3.5 border border-divider p-5 rounded">
    <p class="text-xl">{{ type }}</p>
    <div class="gap-2.5">
      <div class="flex flex-row gap-3">
        <p class="text-gray-400">Лот</p>
        <p>{{ lotName }}</p>
      </div>
      <div class="flex flex-row gap-3">
        <p class="text-gray-400">Участник</p>
        <p>{{ userName }}</p>
      </div>
    </div>
    <div class="flex flex-row gap-3">
      <p class="text-gray-400">Ставка</p>
      <p>{{ bet }}₽</p>
    </div>
    <MyButton class="w-[140px]">Перейти</MyButton>
  </div>
</template>
