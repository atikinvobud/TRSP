<script setup lang="ts">
  import BidList from '@/components/BidList.vue'
  import api from '@/services/axios'
  import { useUserStore } from '@/stores/user-store'
  import type { ILotInfo } from '@/types/lot'
  import { getImageUrl } from '@/utils/api'
  import { formatIsoDate } from '@/utils/formatDate'
  import { storeToRefs } from 'pinia'
  import { onMounted, ref } from 'vue'
  import { useRoute } from 'vue-router'

  const route = useRoute()
  const userStore = useUserStore()

  const { userName } = storeToRefs(userStore)

  const lotInfo = ref<ILotInfo>()

  const fetchLotInfo = async () => {
    try {
      const lotId: number = Number(route.params.id)

      const response = await api.get(`/picture/getfull/${lotId}`)

      lotInfo.value = {
        lot: {
          id: response.data.id,
          name: response.data.name,
          imageUrl: response.data.pictureId,
          author: response.data.author,
          year: response.data.year,
          seller: response.data.seller,
          startDate: formatIsoDate(response.data.dateOfStart),
          endDate: formatIsoDate(response.data.dateOfEnd),
          price: response.data.startPrice,
          leader: null,
          amountOfBids: response.data.bids.length,
        },
        bids: response.data.bids,
      }

      lotInfo.value.lot.imageUrl = await getImageUrl(lotInfo.value.lot.imageUrl)
    } catch (err) {
      console.log(err)
    }
  }

  onMounted(fetchLotInfo)
</script>
<template>
  <main class="py-[36px]">
    <h2 class="text-[32px] font-semibold mb-[36px]">Информация о лоте</h2>
    <section class="flex gap-[60px] mb-[50px]">
      <div class="border border-divider">
        <img class="w-[590px] h-[332px] object-contain" :src="lotInfo?.lot.imageUrl" alt="" />
      </div>
      <div class="py-[20px]">
        <h3 class="text-[26px] font-semibold mb-[26px]">{{ lotInfo?.lot.name }}</h3>
        <div class="flex flex-col gap-[8px] mb-[18px]">
          <p class="text-gray">Автор</p>
          <p class="text-[20px]">{{ lotInfo?.lot.author }}</p>
        </div>
        <div>
          <p class="text-gray">Год</p>
          <p class="text-[24px]">{{ lotInfo?.lot.year }}</p>
        </div>
      </div>
    </section>
    <div class="flex gap-[136px]">
      <section class="flex-1 flex flex-col">
        <h4 class="text-[24px] font-semibold mb-[36px]">Ставки</h4>
        <BidList :bids="lotInfo?.bids"></BidList>
        <button
          v-if="userName !== lotInfo?.lot.seller"
          class="bg-red text-white text-[17px] rounded-[8px] px-[32px] py-[8px] self-center mt-[40px] cursor-pointer hover:bg-[#ff1f1f] transition"
        >
          Сделать ставку
        </button>
      </section>
      <section class="w-[400px]">
        <h4 class="text-[24px] font-semibold mb-[36px]">Информация об аукционе</h4>
        <div class="flex gap-[12px] items-center mb-[12px]">
          <p class="text-gray">Продавец</p>
          <p class="text-[18px]">{{ lotInfo?.lot.seller }}</p>
        </div>
        <div class="flex gap-[12px] items-center mb-[12px]">
          <p class="text-gray">Стартовая цена</p>
          <p class="text-[18px]">{{ lotInfo?.lot.price }}₽</p>
        </div>
        <div class="flex gap-[12px] items-center mb-[36px]">
          <p class="text-gray">Время открытия аукциона</p>
          <p class="text-[18px]">{{ lotInfo?.lot.startDate }}</p>
        </div>
        <div class="flex gap-[12px] items-center">
          <p class="text-gray">Время открытия аукциона</p>
          <p class="text-[18px]">{{ lotInfo?.lot.endDate }}</p>
        </div>
      </section>
    </div>
  </main>
</template>
