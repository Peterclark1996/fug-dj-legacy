import classes from './Input.module.scss'

type InputProps = {
    className?: string,
    value: string,
    onChange: (value: string) => void,
    placeholder: string,
    isValid?: boolean,
    isEnabled?: boolean
}

const Input = ({ className = "", value, onChange, placeholder, isValid = true, isEnabled = true }: InputProps) => {
    return (
        <input
            className={`${className} rounded ${classes.inputField} ${!isValid && classes.inputInvalid}`}
            placeholder={placeholder}
            value={value}
            type="text"
            onChange={event => onChange(event.target.value)}
            disabled={!isEnabled}
        />
    )
}

export default Input