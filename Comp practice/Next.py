n = int(input())
permutation = list(map(int, input().split()))

# Ищем индекс i
i = n - 2
while i >= 0 and permutation[i] > permutation[i + 1]:
    i -= 1

# Если не нашли такую пару, выводим минимальную перестановку
if i == -1:
    permutation.sort()
    print(*permutation)
else:
    # Ищем индекс j
    j = n - 1
    while permutation[j] < permutation[i]:
        j -= 1

    # Меняем местами элементы i и j
    permutation[i], permutation[j] = permutation[j], permutation[i]

    # Сортируем элементы справа от i
    permutation[i + 1:] = sorted(permutation[i + 1:])

    print(*permutation)