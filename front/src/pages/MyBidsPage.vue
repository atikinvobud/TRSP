<script setup lang="ts">
  import LotList from '@/components/LotList.vue'
</script>

<template>
  <main class="py-[36px]">
    <h2 class="text-[32px] font-semibold mb-[36px]">Мои ставки</h2>
    <LotList purpose="get-my-bids"></LotList>
  </main>
</template>
