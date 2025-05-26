import api from '@/services/axios'

export async function getImageUrl(pictureId: string) {
  const response = await api.get<Blob>(`images/${pictureId}`, {
    responseType: 'blob',
  })

  const blob = response.data
  return URL.createObjectURL(blob)
}
