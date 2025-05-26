<script setup lang="ts">
  import { useUserStore } from '@/stores/user-store'
  import { storeToRefs } from 'pinia'
  import { useRouter } from 'vue-router'

  const userStore = useUserStore()
  const { isAuth, userName } = storeToRefs(userStore)

  const router = useRouter()

  const goToLoginPage = () => {
    router.push({ name: 'LoginPage' })
  }
</script>

<template>
  <header class="shadow-[0_4px_5px_rgba(0,0,0,0.03)]">
    <section class="w-[1196px] h-[88px] mx-auto flex justify-between items-center">
      <div class="flex gap-[226px] items-center">
        <RouterLink :to="{ name: 'MainPage' }">
          <h1 class="text-[24px] font-semibold tracking-[0.18em] uppercase cursor-pointer">
            Giga Auction
          </h1>
        </RouterLink>
        <nav v-if="isAuth" class="flex gap-[52px]">
          <RouterLink :to="{ name: 'MyLotsPage' }">
            <p class="text-[18px] cursor-pointer">Мои лоты</p>
          </RouterLink>
          <RouterLink :to="{ name: 'MyBidsPage' }">
            <p class="text-[18px] cursor-pointer">Мои ставки</p>
          </RouterLink>
          <RouterLink :to="{ name: 'NotificationsPage' }">
            <p class="text-[18px] cursor-pointer">Уведомления</p>
          </RouterLink>
        </nav>
      </div>
      <button
        @click="goToLoginPage"
        v-if="!isAuth"
        class="px-[48px] py-[10px] rounded-[6px] bg-black text-[17px] font-semibold text-white cursor-pointer hover:bg-[rgba(0,0,0,0.95)]"
      >
        Войти
      </button>
      <p v-else class="text-[22px] font-bold">{{ userName }}</p>
    </section>
  </header>
</template>
