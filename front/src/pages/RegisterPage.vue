<script setup lang="ts">
  import MyInput from '@/components/MyInput.vue'
  import MyButton from '@/components/MyButton.vue'
  import { onMounted, ref } from 'vue'
  import api from '@/services/axios.ts'
  import { useUserStore } from '@/stores/user-store'
  import { useRouter } from 'vue-router'

  const router = useRouter()

  const email = ref('')
  const login = ref('')
  const password = ref('')
  const message = ref('')
  const messageColor = ref('text-red-500')

  async function register() {
    try {
      const response = await api.post('/auth/register', {
        login: email.value,
        password: password.value,
        nickName: login.value,
      })

      message.value = 'Успешная регистрация'
      messageColor.value = 'text-green-500'

      setTimeout(() => {
        router.push({ name: 'LoginPage' })
      }, 1500)
    } catch (error) {
      message.value = 'Ошибка регистрации'
    }
  }

  async function handleSubmit() {
    const socket = new WebSocket("ws://localhost:5123/ws");
    if (email.value.trim() === '' || login.value.trim() === '' || password.value.trim() === '') {
      message.value = 'Все поля должны быть заполнены'
      return
    }

    await register()
  }


</script>

<template>
  <form @submit.prevent="handleSubmit" class="min-h-screen">
    <div class="flex flex-col shadow-sm rounded w-1/3 p-7 gap-6 mx-auto mt-[40px]">
      <p class="text-center text-[20px]">GigaAuthentication</p>
      <Transition
        enter-active-class="transition-all duration-500 ease-in-out"
        enter-from-class="opacity-0 max-h-0"
        enter-to-class="opacity-100 max-h-40"
        leave-active-class="transition-all duration-500 ease-in-out"
        leave-from-class="opacity-100 max-h-40"
        leave-to-class="opacity-0 max-h-0"
      >
        <p
          v-if="message"
          :class="['overflow-hidden font-russo leading-none text-[16px] text-center', messageColor]"
        >
          {{ message }}
        </p>
      </Transition>
      <div class="flex flex-col gap-[8px]">
        <p>Логин</p>
        <MyInput v-model="email" inputType="text" placeholder="Concord@BestGame.com"></MyInput>
      </div>
      <div class="flex flex-col gap-[8px]">
        <p>Ник</p>
        <MyInput v-model="login" inputType="text" placeholder="ConcordForever"></MyInput>
      </div>
      <div class="flex flex-col gap-[8px]">
        <p>Пароль</p>
        <MyInput v-model="password" inputType="text" placeholder="********" isPassword></MyInput>
      </div>
      <MyButton type="submit" class="w-[280px] mt-5">Зарегистрироваться</MyButton>
      <router-link :to="{ name: 'LoginPage' }">
        <p class="text-gray-400 underline mt-1 text-center">Есть аккаунт?</p>
      </router-link>
    </div>
  </form>
</template>