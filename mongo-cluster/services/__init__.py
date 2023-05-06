def convert_comma_to_number(num: str):
    result = ""
    for i in num:
        if i != ",":
            result += i
    return result
