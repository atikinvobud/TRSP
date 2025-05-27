<script setup lang="ts">
  import type { ILotShortInfo } from '@/types/lot'
  import { differenceBetweenDates } from '@/utils/formatDate'
  import { computed, onMounted, ref } from 'vue'
  import { getImageUrl } from '@/utils/api'
  import { useRouter } from 'vue-router'

  const props = defineProps<{
    purpose: string
    lot: ILotShortInfo
  }>()

  const router = useRouter()

  const imageUrl = ref<string>()
  const fetchData = async () => {
    imageUrl.value = await getImageUrl(props.lot.imageUrl)
  }

  const differenceDate = computed(() => {
    return differenceBetweenDates(props.lot.endDate)
  })

  const goToLotPage = () => {
    router.push({ name: 'LotPage', params: { id: props.lot.id } })
  }

  onMounted(fetchData)
</script>

<template>
  <div class="flex gap-[56px]">
    <div class="border border-divider">
      <img class="w-[568px] h-[320px] object-contain" :src="imageUrl" alt="Картина" />
    </div>
    <div class="flex-1 flex flex-col">
      <h3 class="text-[22px] font-bold mb-[16px]">{{ lot.name }}</h3>
      <div class="flex gap-[72px] mb-[20px]">
        <div class="flex flex-col gap-[8px]">
          <p class="text-[14px] text-gray">Художник</p>
          <p class="text-[18px]">{{ lot.author }}</p>
        </div>
        <div class="flex flex-col gap-[8px] items-center">
          <p class="text-[14px] text-gray">Год</p>
          <p class="text-[18px]">{{ lot.year }}</p>
        </div>
      </div>
      <div class="flex-1 border-t border-t-divider pt-[16px] flex justify-between">
        <div class="flex flex-col justify-between">
          <div class="flex flex-col gap-[10px]">
            <div v-if="purpose !== 'get-my-lots'" class="flex gap-[12px] items-center">
              <p class="text-gray text-[14px]">Продавец</p>
              <p>{{ lot.seller }}</p>
            </div>
            <div class="flex gap-[12px] items-center">
              <p class="text-gray text-[14px]">Ставки</p>
              <p>{{ lot.amountOfBids }}</p>
            </div>
            <div v-if="lot.leader !== null" class="flex gap-[12px] items-center">
              <p class="text-gray text-[14px]">{{ differenceDate != '' ? 'Лидер' : 'Куплено' }}</p>
              <p>{{ lot.leader }}</p>
            </div>
          </div>
          <div>
            <p class="text-gray text-[14px]">
              {{
                lot.amountOfBids !== 0
                  ? 'Текущая ставка'
                  : differenceDate !== ''
                    ? 'Минимальная ставка'
                    : 'Цена покупки'
              }}
            </p>
            <p class="text-[26px]">{{ lot.price }}₽</p>
          </div>
        </div>
        <div class="flex flex-col justify-between items-center">
          <div v-if="differenceDate !== ''" class="flex flex-col items-center gap-[6px]">
            <p class="text-gray text-[14px]">До конца аукциона</p>
            <p class="text-[18px] font-semibold">{{ differenceDate }}</p>
          </div>
          <p v-else class="text-[18px] font-semibold">Аукцион завершён</p>
          <button
            @click="goToLotPage"
            class="bg-red text-white text-[16px] font-semibold rounded-[8px] w-[210px] py-[9px] cursor-pointer hover:bg-[#ff1f1f] transition"
          >
            Подробнее
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
