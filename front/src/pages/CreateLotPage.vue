<script setup lang="ts">
  import VueDatePicker from '@vuepic/vue-datepicker'
  import '@vuepic/vue-datepicker/dist/main.css'
  import { ref } from 'vue'
  import { formatDates } from '@/utils/formatDate'
  import type { ILotForm } from '@/types/lot'
  import api from '@/services/axios'
  import { useRouter } from 'vue-router'

  const router = useRouter()

  const form = ref<ILotForm>({
    name: '',
    author: '',
    year: null,
    date: [],
    startPrice: null,
    image: null,
  })

  const imageUrl = ref<string | null>(null)
  const fileInput = ref<HTMLInputElement>()

  const changeFile = (event: Event) => {
    const target = event.target as HTMLInputElement
    const file = target.files?.[0]
    if (file) {
      imageUrl.value = URL.createObjectURL(file)

      form.value.image = file
    }
  }

  const targetFileInput = () => {
    fileInput.value?.click()
  }

  const handleForm = async () => {
    try {
      if (form.value.image === null) return

      const image = new FormData()
      image.append('file', form.value.image)

      const imageResponse = await api.post<{ id: string }>('images/upload', image, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })

      const rawBody = {
        name: form.value.name,
        author: form.value.author,
        year: form.value.year,
        dateOfStart: form.value.date[0],
        dateOfEnd: form.value.date[1],
        startPrice: form.value.startPrice,
        pictureId: imageResponse.data.id,
      }
      const response = await api.post('picture/create', rawBody)

      router.push({ name: 'LotPage', params: { id: response.data.id } })
    } catch (err) {
      console.log(err)
    }
  }
</script>

<template>
  <main class="py-[36px]">
    <h2 class="text-[32px] font-semibold mb-[33px]">Создание лота</h2>
    <form @submit.prevent="handleForm" class="flex justify-between">
      <section class="w-[400px] flex flex-col">
        <div class="flex flex-col gap-[16px] mb-[24px]">
          <label for="name" class="font-normal">Название картины</label>
          <input
            v-model="form.name"
            type="text"
            name="name"
            required
            placeholder="Если заблудился в лесу, иди домой"
            class="px-[12px] py-[8px] border border-divider rounded-[4px]"
          />
        </div>
        <div class="flex flex-col gap-[16px] mb-[24px]">
          <label for="author" class="font-normal">Фамилия и имя автора</label>
          <input
            v-model="form.author"
            type="text"
            name="author"
            required
            placeholder="Стэтхем Джейсон"
            class="px-[12px] py-[8px] border border-divider rounded-[4px]"
          />
        </div>
        <div class="flex flex-col gap-[16px] mb-[48px]">
          <label for="year" class="font-normal">Год написания картины</label>
          <input
            v-model="form.year"
            type="number"
            name="year"
            placeholder="2007"
            required
            class="px-[12px] py-[8px] border border-divider rounded-[4px]"
          />
        </div>
        <div class="flex flex-col gap-[16px] mb-[24px]">
          <label for="date" class="font-normal">Дата проведения аукциона</label>
          <VueDatePicker
            v-model="form.date"
            range
            model-type="yyyy-MM-dd'T'HH:mm"
            select-text="Выбрать"
            cancel-text="Отмена"
            :clearable="false"
            placeholder="Выберите дату"
            no-today
            required
            name="date"
            :format="formatDates"
            locale="ru"
            month-name-format="long"
            position="left"
          ></VueDatePicker>
        </div>
        <div class="flex flex-col gap-[16px] mb-[56px]">
          <label for="price" class="font-normal">Стартовая цена</label>
          <input
            v-model="form.startPrice"
            type="number"
            name="price"
            placeholder="100"
            required
            class="px-[12px] py-[8px] border border-divider rounded-[4px]"
          />
        </div>
        <button
          type="submit"
          class="bg-red text-white rounded-[8px] px-[56px] py-[9px] self-center cursor-pointer hover:bg-[#ff1f1f]"
        >
          Создать лот
        </button>
      </section>
      <section>
        <input
          type="file"
          accept=".jpg,.jpeg, .png, .webp"
          ref="fileInput"
          @change="changeFile"
          required
          hidden
        />
        <div
          v-if="imageUrl === null"
          class="w-[568px] h-[320px] rounded-[4px] bg-[#EDEDED] border border-divider cursor-pointer flex justify-center items-center"
          @click="targetFileInput"
        >
          <svg
            width="36"
            height="36"
            viewBox="0 0 36 36"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <g clip-path="url(#clip0_95_2522)">
              <path
                d="M28 36H7.99998C5.86324 36 3.85429 35.1678 2.34319 33.6567C0.832156 32.1459 0 30.1368 0 28V25.9999C0 24.8952 0.895384 23.9998 2.00001 23.9998C3.10464 23.9998 4.00002 24.8952 4.00002 25.9999V28C4.00002 29.0684 4.41614 30.0728 5.17151 30.8283C5.9271 31.5838 6.93154 32 7.99998 32H28C29.0683 32 30.0728 31.5838 30.8283 30.8283C31.5838 30.0727 32 29.0683 32 28V25.9999C32 24.8952 32.8954 23.9998 33.9999 23.9998C35.1045 23.9998 36 24.8952 36 25.9999V28C36 30.1367 35.1678 32.1456 33.6567 33.6567C32.1456 35.1678 30.1367 36 28 36ZM18 27.9999C17.4777 28.0006 16.9761 27.7963 16.6028 27.431L16.6027 27.4309L16.5987 27.427L16.5976 27.4259C16.5965 27.4249 16.5955 27.4239 16.5944 27.4228L16.5924 27.4208C16.5917 27.42 16.5909 27.4193 16.5902 27.4186L16.5858 27.4143L8.58579 19.4142C7.80477 18.6331 7.80477 17.3668 8.58579 16.5857C9.36681 15.8047 10.6332 15.8046 11.4143 16.5857L16 21.1715V2.00001C16 0.895384 16.8953 0 18 0C19.1046 0 20 0.895384 20 2.00001V21.1714L24.5857 16.5857C25.3667 15.8047 26.6333 15.8047 27.4142 16.5857C28.1952 17.3667 28.1952 18.6331 27.4142 19.4142L19.4142 27.4141L19.4098 27.4185L19.4076 27.4206L19.4056 27.4227C19.4046 27.4238 19.4035 27.4248 19.4025 27.4257L19.4013 27.4269C19.4 27.4282 19.3987 27.4295 19.3974 27.4308L19.3973 27.4309C19.3752 27.4524 19.353 27.4732 19.3302 27.4934C19.1669 27.6393 18.9807 27.7572 18.779 27.8424L18.777 27.8432C18.7762 27.8435 18.7756 27.8439 18.7748 27.8442C18.5295 27.9473 18.2661 28.0003 18 27.9999Z"
                fill="#727272"
              />
            </g>
            <defs>
              <clipPath id="clip0_95_2522">
                <rect width="36" height="36" fill="white" />
              </clipPath>
            </defs>
          </svg>
        </div>
        <img
          v-else
          :src="imageUrl"
          @click="targetFileInput"
          class="w-[568px] h-[320px] object-contain cursor-pointer"
        />
      </section>
    </form>
  </main>
</template>

<style scoped>
  ::v-deep(.dp__input_icon) {
    right: 5px;
    left: auto;
  }

  ::v-deep(.dp__input) {
    padding: 8px 12px;
    border-radius: 4px;
    border-color: rgba(0, 0, 0, 0.2);
    font-family: 'Montserrat';
  }
</style>
