<script setup lang="ts">
import MyInput from '@/components/MyInput.vue'
import MyButton from '@/components/MyButton.vue'
import {ref} from 'vue'
import api from '@/services/axios.ts'
import { useRouter } from 'vue-router'
const email = ref('')
const password = ref('')
const message = ref('')
const messageColor = ref('text-red-500')

const router = useRouter()

async function login() {
    try {
        const response = await api.post(
            '/auth/login',
            null,
            {
                params: {
                    login: email.value,
                    password: password.value,
                }
            }
        )

        message.value = 'Успешный вход'
        messageColor.value = 'text-green-500'

        setTimeout(() => {
            router.push('/')
        }, 1500)

    } catch (error) {
        message.value = 'Ошибка авторизации'
    }
}

async function handleSubmit() {
    if(email.value.trim() === '' || password.value.trim() === '')
    {
        message.value = 'Все поля должны быть заполнены'
        return;
    }

    await login()
}
</script>

<template>
    <form @submit.prevent="handleSubmit" class="min-h-screen flex justify-center items-center">
        <div class="flex flex-col shadow-sm rounded w-1/4 p-7 gap-5">
            <p class="text-center">GigaTest</p>
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
                    :class="['overflow-hidden font-russo leading-none text-xs text-center', messageColor]"
                >
                    {{ message }}
                </p>
            </Transition>
            <div>
                <p>Email:</p>
                <MyInput v-model="email" inputType="text" placeholder="Concord@BestGame.com"></MyInput>
            </div>
            <div>
                <p>Password:</p>
                <MyInput v-model="password" inputType="text" placeholder="********" isPassword></MyInput>
            </div>
            <div>
                <MyButton type="submit" class="mt-5 w-full">Войти</MyButton>
                <router-link :to="{ name: 'RegisterPage' }">
                    <p class="text-gray-400 underline mt-1 text-center">Не зарегистрированны?</p>
                </router-link>
            </div>
        </div>
    </form>
</template>