def tek_cift_ayir(liste):
    tek_sayilar = []
    cift_sayilar = []

    for sayi in liste:
        if sayi % 2 == 0: 
            cift_sayilar.append(sayi)
        else:
            tek_sayilar.append(sayi)

    if not tek_sayilar:
        en_buyuk_tek = None
    else:
        en_buyuk_tek = max(tek_sayilar)

    if not cift_sayilar:
        en_kucuk_cift = None
    else:
        en_kucuk_cift = min(cift_sayilar)

    return en_buyuk_tek, en_kucuk_cift

liste = [1, 2, 3, 4, 5, 6, 7, 8, 9]
en_buyuk_tek, en_kucuk_cift = tek_cift_ayir(liste)
print("En büyük tek sayı:", en_buyuk_tek)
print("En küçük çift sayı:", en_kucuk_cift)
