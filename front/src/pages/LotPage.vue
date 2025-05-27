<script setup lang="ts">
  import BidList from '@/components/BidList.vue'
  import api from '@/services/axios'
  import { useUserStore } from '@/stores/user-store'
  import type { ILotInfo } from '@/types/lot'
  import { getImageUrl } from '@/utils/api'
  import { differenceBetweenDates, formatIsoDate } from '@/utils/formatDate'
  import { storeToRefs } from 'pinia'
  import { computed, onMounted, ref } from 'vue'
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
          startDate: response.data.dateOfStart,
          endDate: response.data.dateOfEnd,
          price: response.data.startPrice,
          leader: null,
          amountOfBids: response.data.bids.length,
        },
        bids: response.data.bids.map((b) => ({
          dateTime: b.date,
          userName: b.user,
          price: b.price,
        })),
      }

      lotInfo.value.lot.imageUrl = await getImageUrl(lotInfo.value.lot.imageUrl)
    } catch (err) {
      console.log(err)
    }
  }

  const differenceDate = computed(() => {
    if (lotInfo.value !== undefined) return differenceBetweenDates(lotInfo.value.lot.endDate)

    return ''
  })

  const isOpen = ref<boolean>(false)

  const bid = ref<number>()
  const handleForm = async () => {
    if (bid === undefined) return

    try {
      const rawBody = {
        price: bid.value,
        date: new Date().toISOString(),
        pictureId: lotInfo.value?.lot.id,
      }

      await api.post('/bet/create', rawBody)

      isOpen.value = false
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
        <BidList :bids="lotInfo?.bids || []"></BidList>
        <button
          @click="isOpen = true"
          v-if="userName !== lotInfo?.lot.seller && differenceDate !== ''"
          class="bg-red text-white text-[17px] rounded-[8px] px-[32px] py-[8px] self-center mt-[40px] cursor-pointer hover:bg-[#ff1f1f] transition"
        >
          Сделать ставку
        </button>
        <p
          v-else-if="differenceDate === ''"
          class="mt-[40px] underline underline-offset-[0.25em] text-[22px] text-center"
        >
          Лот продан
        </p>
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
          <p class="text-[18px]">{{ formatIsoDate(lotInfo?.lot.startDate) }}</p>
        </div>
        <div class="flex gap-[12px] items-center">
          <p class="text-gray">Время открытия аукциона</p>
          <p class="text-[18px]">{{ formatIsoDate(lotInfo?.lot.endDate) }}</p>
        </div>
      </section>
    </div>
    <div v-if="isOpen" class="fixed inset-0 bg-[rgba(0,0,0,0.5)] flex justify-center items-center">
      <div class="relative bg-white w-[470px] rounded-[4px] p-[24px]">
        <p class="text-gray text-[14px] mb-[8px]">Картина</p>
        <h4 class="text-[22px] mb-[12px]">Утро в сосновом лесу</h4>
        <div class="flex gap-[12px] mb-[32px] items-center">
          <p class="text-gray text-[14px]">Текущая цена</p>
          <p>14₽</p>
        </div>
        <form @submit.prevent="handleForm" class="flex justify-between items-center gap-[40px]">
          <input
            v-model="bid"
            type="number"
            :min="lotInfo?.lot.price"
            class="w-[172px] border border-[rgba(0,0,0,0.8)] px-[8px] py-[4px] rounded-[2px] text-[20px]"
            placeholder="₽"
          />
          <button
            type="submit"
            class="bg-red text-white py-[12px] flex-1 rounded-[8px] cursor-pointer hover:bg-[#ff1f1f] transition"
          >
            Сделать ставку
          </button>
        </form>
        <div
          @click="isOpen = false"
          class="absolute right-[16px] top-[16px] cursor-pointer hover:scale-102 transition"
        >
          <svg
            width="20"
            height="20"
            viewBox="0 0 20 20"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M0.207699 19.7905C0.274034 19.8569 0.352807 19.9096 0.439516 19.9456C0.526225 19.9815 0.619168 20 0.713032 20C0.806896 20 0.899839 19.9815 0.986548 19.9456C1.07326 19.9096 1.15203 19.8569 1.21836 19.7905L9.99652 11.0124L18.7782 19.7905C18.9123 19.9245 19.094 19.9998 19.2836 19.9998C19.4731 19.9998 19.6549 19.9245 19.7889 19.7905C19.9229 19.6565 19.9982 19.4747 19.9982 19.2852C19.9982 19.0956 19.9229 18.9139 19.7889 18.7799L11.0072 10.0017L19.7853 1.21998C19.9194 1.08596 19.9946 0.904185 19.9946 0.714649C19.9946 0.525112 19.9194 0.343338 19.7853 0.209316C19.6513 0.0752931 19.4695 0 19.28 0C19.0905 0 18.9087 0.0752931 18.7747 0.209316L9.99652 8.99104L1.21479 0.212887C1.07816 0.0958738 0.902396 0.0347295 0.722636 0.0416729C0.542876 0.0486162 0.372356 0.123136 0.245152 0.25034C0.117948 0.377544 0.0434283 0.548064 0.036485 0.727824C0.0295416 0.907584 0.0906859 1.08334 0.207699 1.21998L8.98585 10.0017L0.207699 18.7834C0.0746691 18.9172 0 19.0983 0 19.287C0 19.4757 0.0746691 19.6567 0.207699 19.7905Z"
              fill="black"
              fill-opacity="0.9"
            />
          </svg>
        </div>
      </div>
    </div>
  </main>
</template>
