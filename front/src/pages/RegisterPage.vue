<script setup lang="ts">
import MyInput from '@/components/MyInput.vue'
import MyButton from '@/components/MyButton.vue'
import {ref} from 'vue'
import api from '@/services/axios.ts'
import { useRouter } from 'vue-router'

const router = useRouter()

const email = ref('')
const login = ref('')
const password = ref('')
const message = ref('')
const messageColor = ref('text-red-500')

async function register() {
    try {
        const response = await api.post(
            '/auth/register',
            {
                login: email.value,
                password: password.value,
                nickName: login.value,
            }
        )

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
    if(email.value.trim() === '' || login.value.trim() === '' || password.value.trim() === '')
    {
        message.value = 'Все поля должны быть заполнены'
        return;
    }

    await register()
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
                <p>Login:</p>
                <MyInput v-model="login" inputType="text" placeholder="ConcordForever"></MyInput>
            </div>
            <div>
                <p>Password:</p>
                <MyInput v-model="password" inputType="text" placeholder="********" isPassword></MyInput>
            </div>
            <div>
                <MyButton type="submit" class="mt-5 w-full">Зарегистрироваться</MyButton>
                <router-link :to="{ name: 'LoginPage' }">
                    <p class="text-gray-400 underline mt-1 text-center">Есть аккаунт?</p>
                </router-link>
            </div>
        </div>
    </form>
</template>