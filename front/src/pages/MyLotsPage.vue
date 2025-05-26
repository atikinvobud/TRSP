<script setup lang="ts">
  import LotList from '@/components/LotList.vue'
  import { useRouter } from 'vue-router'

  const router = useRouter()
  const goToCreateLotPage = () => {
    router.push({ name: 'CreateLotPage' })
  }
</script>

<template>
  <main class="py-[36px] relative">
    <h2 class="text-[32px] font-semibold mb-[36px]">Мои лоты</h2>
    <LotList purpose="get-my-lots"></LotList>
    <button
      @click="goToCreateLotPage"
      class="absolute right-[20px] top-[36px] bg-black text-white font-semibold text-[17px] w-[200px] py-[10px] rounded-[8px] cursor-pointer hover:bg-[rgba(0,0,0,0.95)]"
    >
      Создать лот
    </button>
  </main>
</template>
