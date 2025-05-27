<script setup lang="ts">
  import type { IBidInfo } from '@/types/lot'
  import BidElement from './BidElement.vue'

  defineProps<{
    bids: IBidInfo[]
  }>()
</script>

<template>
  <ul class="flex pb-[16px] border-b border-b-divider">
    <li class="text-gray basis-1/12">№</li>
    <li class="text-gray basis-2/12 text-center">Время</li>
    <li class="text-gray basis-6/12 text-center">Пользователь</li>
    <li class="text-gray basis-3/12 text-center">Сумма ставки</li>
  </ul>
  <ul class="">
    <BidElement
      v-for="(bid, index) in bids"
      :key="index"
      :index="bids.length - index + 1"
      :bid="bid"
    ></BidElement>
  </ul>
</template>
