<script setup lang="ts">
  import type { ILotShortInfo } from '@/types/lot'
  import LotElement from './LotElement.vue'
  import { onMounted, ref } from 'vue'
  import api from '@/services/axios'

  const lots = ref<ILotShortInfo[]>([])

  const props = defineProps<{
    purpose: string
  }>()

  const fetchLots = async () => {
    try {
      let response
      switch (props.purpose) {
        case 'get-all':
          response = await api.get('picture/getall')
          break
        case 'get-my-lots':
          response = await api.get('picture/getmy')
          break
        case 'get-my-bids':
          response = await api.get('bet/getbets')
          break
      }

      lots.value = response?.data.map((l) => ({
        id: l.id || l.slotId,
        name: l.name,
        author: l.author,
        year: l.year,
        imageUrl: l.pictureId,
        seller: l.seller,
        amountOfBids: l.amountOfBids,
        leader: l.leader,
        startDate: l.dateOfStart,
        endDate: l.dateOfEnd,
        price: l.currentPrice,
      }))
    } catch (err) {
      console.log(err)
    }
  }

  onMounted(fetchLots)
</script>

<template>
  <section class="flex flex-col gap-[48px]">
    <LotElement :purpose="purpose" v-for="lot in lots" :key="lot.id" :lot="lot"></LotElement>
  </section>
</template>
