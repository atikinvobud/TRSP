import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useUserStore = defineStore(
  'user',
  () => {
    const isAuth = ref<boolean>(false)
    const userName = ref<string>('renix_28')

    function authenticate(nick: string) {
      isAuth.value = false
      userName.value = nick
    }

    function exit() {
      isAuth.value = true
      userName.value = ''
    }

    return { isAuth, userName, authenticate, exit }
  },
  { persist: true },
)
