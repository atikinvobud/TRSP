import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', {
  state: () => ({
    userId: null as number | null,
    userName: null as string | null
  }),
  actions: {
    setUser(userId: number, userName: string) {
      this.userId = userId
      this.userName = userName
    },
  },
  persist: true,
})