<script setup lang="ts">
import NotificationBlock from '@/components/NotificationBlock.vue';
const notifications = [
  {
    id: 1,
    type: 'Новая ставка',
    lotName: 'Сосиски в тесте',
    userName: 'HelloThere',
    bet: 100,
  },
  {
    id: 2,
    type: 'Новый релиз',
    lotName: 'Death Stranding 2',
    userName: 'GameEnjoyer',
    bet: 200,
  },
  {
    id: 3,
    type: 'Новое предложение',
    lotName: 'Продам соседа',
    userName: 'IHateMyNeightbore',
    bet: 5,
  },
  {
    id: 4,
    type: 'Новый товар',
    lotName: 'RTX 5090',
    userName: 'EasyPC',
    bet: 39999,
  },
  {
    id: 5,
    type: '!1!Срочно!1!',
    lotName: 'Женщины без обязательств рядом',
    userName: 'NotPornHub.com',
    bet: 20,
  },
]
</script>

<template>
  <div>
    <p class="text-[32px] font-semibold mt-10">Уведомления</p>
    <div class="grid grid-cols-4 gap-5 mt-10">
    <NotificationBlock
    v-for="notification in notifications"
    :key="notification.id"
    :type="notification.type"
    :lot-name="notification.lotName"
    :user-name="notification.userName"
    :bet="notification.bet"
    />
  </div>
  </div>
</template>
