export function formatDates(date: Date[]) {
  const padZeros = (value: number, length: number) => value.toString().padStart(length, '0')

  const startDay = padZeros(date[0].getDate(), 2)
  const startMonth = padZeros(date[0].getMonth() + 1, 2)
  const startYear = date[0].getFullYear()

  const endDay = padZeros(date[1].getDate(), 2)
  const endMonth = padZeros(date[1].getMonth() + 1, 2)
  const endYear = date[1].getFullYear()

  return `${startDay}.${startMonth}.${startYear} - ${endDay}.${endMonth}.${endYear}`
}
