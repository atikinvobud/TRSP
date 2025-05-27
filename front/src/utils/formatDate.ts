const padZeros = (value: number, length: number) => value.toString().padStart(length, '0')

export function formatDates(date: Date[]) {
  const startDay = padZeros(date[0].getDate(), 2)
  const startMonth = padZeros(date[0].getMonth() + 1, 2)
  const startYear = date[0].getFullYear()
  const startHours = padZeros(date[0].getHours(), 2)
  const startMinutes = padZeros(date[0].getMinutes(), 2)

  const endDay = padZeros(date[1].getDate(), 2)
  const endMonth = padZeros(date[1].getMonth() + 1, 2)
  const endYear = date[1].getFullYear()
  const endHours = padZeros(date[1].getHours(), 2)
  const endMinutes = padZeros(date[1].getMinutes(), 2)

  return `${startDay}.${startMonth}.${startYear} ${startHours}:${startMinutes} - ${endDay}.${endMonth}.${endYear} ${endHours}:${endMinutes}`
}

export function formatIsoDate(isoDate: string) {
  const date = new Date(isoDate)

  const day = padZeros(date.getDate(), 2)
  const month = padZeros(date.getMonth() + 1, 2)
  const year = date.getFullYear()
  const hours = padZeros(date.getHours() - 3, 2)
  const minutes = padZeros(date.getMinutes(), 2)

  return `${day}.${month}.${year} ${hours}:${minutes} `
}

export function differenceBetweenDates(endDateIso: string) {
  const endDate = new Date(endDateIso)
  const now = new Date()

  const diff = endDate.getTime() - now.getTime()

  if (diff <= 0) {
    return ''
  }

  const minutes = Math.floor(diff / 1000 / 60) % 60
  const hours = Math.floor(diff / 1000 / 60 / 60) % 24
  const days = Math.floor(diff / 1000 / 60 / 60 / 24)

  const stringMinutes = padZeros(Math.floor(diff / 1000 / 60) % 60, 2)
  const stringHours = padZeros(Math.floor(diff / 1000 / 60 / 60) % 24, 2)

  if (days >= 1) {
    return `${days} д ${stringHours} ч ${stringMinutes} мин`
  } else if (hours >= 1) {
    return `${hours} ч ${stringMinutes} мин`
  } else {
    return `${minutes} мин`
  }
}
